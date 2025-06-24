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


builder.Services.AddIdentity<ApplicationUser, IdentityRole<Guid>>()
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
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();
    dbContext.Database.EnsureCreated();
    await DataSeeder.SeedRolesAsync(roleManager);
    await DataSeeder.SeedUsersAsync(userManager);

    if (await scopeManager.FindByNameAsync("quiz_api") is null)
    {
        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = "quiz_api",
            DisplayName = "Quiz API access",
            Resources = { "quizapi" } 
        });
    }

    if (await scopeManager.FindByNameAsync("question_api") is null)
    {
        await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
        {
            Name = "question_api",
            DisplayName = "Question API access",
            Resources = { "questionapi" } 
        });
    }

    var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    var swaggerClientConfig = openIddictConfig.GetSection("Clients:SwaggerUI");
    var swaggerClientId = swaggerClientConfig["ClientId"];

    if (await manager.FindByClientIdAsync(swaggerClientId) is null)
    {
        var descriptor = new OpenIddictApplicationDescriptor
        {
            ClientId = swaggerClientId,
            DisplayName = swaggerClientConfig["DisplayName"],
            ClientType = ClientTypes.Public,
            ConsentType = ConsentTypes.Implicit,
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
            
                Permissions.Prefixes.Scope + "quiz_api",
                Permissions.Prefixes.Scope + "question_api"
            },
            Requirements =
            {
                Requirements.Features.ProofKeyForCodeExchange
            }
        };

        var redirectUris = swaggerClientConfig.GetSection("RedirectUris").Get<string[]>()
                                            .Select(uri => new Uri(uri));
        foreach (var uri in redirectUris)
        {
            descriptor.RedirectUris.Add(uri);
        }

        var postLogoutRedirectUris = swaggerClientConfig.GetSection("PostLogoutRedirectUris").Get<string[]>()
                                                        .Select(uri => new Uri(uri));
        foreach (var uri in postLogoutRedirectUris)
        {
            descriptor.PostLogoutRedirectUris.Add(uri);
        }

        await manager.CreateAsync(descriptor);
    }
}

app.Run();