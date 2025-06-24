using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using UserService.API.Models;

namespace UserService.API.Data
{
    public static class DataSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            
            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
        }

        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            if (await userManager.FindByEmailAsync("admin@quizengine.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@quizengine.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            if (await userManager.FindByEmailAsync("user@quizengine.com") == null)
            {
                var standardUser = new ApplicationUser
                {
                    UserName = "user",
                    Email = "user@quizengine.com",
                    FirstName = "Standard",
                    LastName = "User",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(standardUser, "User@123");
                await userManager.AddToRoleAsync(standardUser, "User");
            }
        }
    }
}