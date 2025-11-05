using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using DevSpot_CodeAlong.Constants;
namespace DevSpot_CodeAlong.Data
{
    public static class UserSeeder
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

           await CreateUserWithRole(userManager, "admin@devspot.com", "Admin!123", Roles.Admin);
           await CreateUserWithRole(userManager, "jobseeker@devspot.com", "Jobseeker!123", Roles.JobSeeker);
           await CreateUserWithRole(userManager, "employer@devspot.com", "Employer!123", Roles.Employer);

        }

        private static async Task CreateUserWithRole(UserManager<IdentityUser> userManager,
            string email, 
            string password, 
            string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                var newUser = new IdentityUser
                {
                    Email = email,
                    EmailConfirmed = true,
                    UserName = email

                };

                var result = await userManager.CreateAsync(newUser, password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, role);
                }
                else
                {
                    throw new Exception($"Failed creating user with Email: {newUser.Email}. Errors: {string.Join(",", result.Errors)}");
                }
            }
        }
    }
}
