using AlicisinaWebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace AlicisinaWebApp.Data
{
    public static class RoleSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roleNames = {"Admin","Member"};

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if(!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }

                var adminEmail = "b12@gmail.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if(adminUser == null)
                {
                    var newAdmin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };
                    var createAdmin = await userManager.CreateAsync(newAdmin,"Admin123!");
                    if(createAdmin.Succeeded)
                    {
                        await userManager.AddToRoleAsync(newAdmin,"Admin");
                    }
                }
            }
        }
    }
}