using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.Models;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseOpenIddict();
});


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var openIddictConfig = builder.Configuration.GetSection("OpenIddict");
// Configure authentication
builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<ApplicationDbContext>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris(openIddictConfig["TokenEndpointUri"] ?? "/connect/token");
        options.SetAuthorizationEndpointUris(openIddictConfig["AuthorizationEndpointUri"] ?? "/connect/authorize");

        options.AllowAuthorizationCodeFlow().RequireProofKeyForCodeExchange();
        options.AllowRefreshTokenFlow();

        options.AcceptAnonymousClients();

        var scopes = openIddictConfig.GetSection("Scopes").Get<string[]>();
        options.RegisterScopes(scopes);
        //Temp approach, should change in production
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        options.DisableAccessTokenEncryption();

        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough()
               .DisableTransportSecurityRequirement();
        options.SetIssuer(builder.Configuration["JwtSettings:Authority"]);
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = builder.Configuration["ApplicationCookie:LoginPath"] ?? "/Identity/Account/Login";
});


var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseCors(o=> { o.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin(); });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapRazorPages();
app.MapDefaultControllerRoute();


// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
    await DataSeeder.SeedRolesAsync(roleManager);
    await DataSeeder.SeedUsersAsync(userManager);

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    var swaggerQuizClientConfig = openIddictConfig.GetSection("Clients:SwaggerQuizUI");

    if (await manager.FindByClientIdAsync(swaggerQuizClientConfig["ClientId"]) is null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = swaggerQuizClientConfig["ClientId"],
            DisplayName = swaggerQuizClientConfig["DisplayName"],
            ClientType = ClientTypes.Public,
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Prefixes.Scope + "quizapi"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            },
            RedirectUris = { new Uri(swaggerQuizClientConfig["RedirectUri"]) },
            PostLogoutRedirectUris = { new Uri(swaggerQuizClientConfig["PostLogoutRedirectUri"]) },
        });
    }

    var swaggerQuestionClientConfig = openIddictConfig.GetSection("Clients:SwaggerQuestionUI");

    if (await manager.FindByClientIdAsync(swaggerQuestionClientConfig["ClientId"]) is null)
    {
        await manager.CreateAsync(new OpenIddictApplicationDescriptor
        {
            ClientId = swaggerQuestionClientConfig["ClientId"],
            DisplayName = swaggerQuestionClientConfig["DisplayName"],
            ClientType = ClientTypes.Public,
            Permissions =
            {
                Permissions.Endpoints.Authorization,
                Permissions.Endpoints.Token,
                Permissions.GrantTypes.AuthorizationCode,
                Permissions.GrantTypes.RefreshToken,
                Permissions.ResponseTypes.Code,
                Permissions.Scopes.Email,
                Permissions.Scopes.Profile,
                Permissions.Scopes.Roles,
                Permissions.Prefixes.Scope + "quizapi"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            },
            RedirectUris = { new Uri(swaggerQuestionClientConfig["RedirectUri"]) },
            PostLogoutRedirectUris = { new Uri(swaggerQuestionClientConfig["PostLogoutRedirectUri"]) },
        });
    }
}

app.Run();