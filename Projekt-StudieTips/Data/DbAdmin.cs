using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
namespace Projekt_StudieTips.Data
{
    public class DbAdmin
    {
        public static void SeedUsers(UserManager<IdentityUser> userManager, ILogger log)
        {
            const string adminEmail = "Admin@localhost";
            const string adminPassword = "Secret7/";

            if (userManager.FindByNameAsync(adminEmail).Result == null)
            {
                log.LogWarning("Seeding the admin user");
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };
                IdentityResult result = userManager.CreateAsync
                    (user, adminPassword).Result;
                if (result.Succeeded)
                {
                    var adminClaim = new Claim("Admin", "ADMINISTRATOR");
                    userManager.AddClaimAsync(user, adminClaim);
                }
            }
        }
    }
}
