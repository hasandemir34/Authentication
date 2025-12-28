using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection; // GetRequiredService için gerekli
using UyeSistemi.Data;

namespace UyeSistemi.Data
{
    public static class SeedIdentityData
    {
        private const string adminUser = "Admin";
        private const string adminPassword = "AdminPassword123$";
        private const string regularUser = "User";
        private const string regularPassword = "UserPassword123$";
        private const string adminRole = "Admin";
        private const string userRole = "User";

        public static void EnsurePopulated(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<UygulamaDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                if (!roleManager.RoleExistsAsync(adminRole).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(adminRole)).Wait();
                }
                if (!roleManager.RoleExistsAsync(userRole).Result)
                {
                    roleManager.CreateAsync(new IdentityRole(userRole)).Wait();
                }

                var admin = userManager.FindByNameAsync(adminUser).Result;
                if (admin == null)
                {
                    var newAdmin = new IdentityUser
                    {
                        UserName = adminUser,
                        Email = "admin@uye.com",
                        EmailConfirmed = true
                    };
                    var result = userManager.CreateAsync(newAdmin, adminPassword).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(newAdmin, adminRole).Wait();
                    }
                }

                var regular = userManager.FindByNameAsync(regularUser).Result;
                if (regular == null)
                {
                    var newUser = new IdentityUser
                    {
                        UserName = regularUser,
                        Email = "user@uye.com",
                        EmailConfirmed = true
                    };
                    var result = userManager.CreateAsync(newUser, regularPassword).Result;
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(newUser, userRole).Wait();
                    }
                }
            }
        }
    }
}