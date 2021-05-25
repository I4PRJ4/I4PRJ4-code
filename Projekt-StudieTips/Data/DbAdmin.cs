using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
namespace Projekt_StudieTips.Data
{
    public class DbAdmin
    {
        public static void SeedAdmin(UserManager<IdentityUser> userManager, ILogger log)
        {
            const string adminEmail = "Admin@localhost";
            const string adminPassword = "Secret7/";
            const string adminName = "Admin";

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
                    var adminClaim = new Claim("Admin", adminName);
                    userManager.AddClaimAsync(user, adminClaim);
                }
            }
        }

        public static void SeedModerator(UserManager<IdentityUser> userManager, ILogger log)
        {
            const string moderatorEmail = "Mod@localhost";
            const string moderatorPassword = "Secret7/";
            const string moderatorName = "Moderator";

            if (userManager.FindByNameAsync(moderatorEmail).Result == null)
            {
                log.LogWarning("Seeding the moderator user");
                var user = new IdentityUser
                {
                    UserName = moderatorEmail,
                    Email = moderatorPassword
                };
                IdentityResult result = userManager.CreateAsync
                    (user, moderatorPassword).Result;
                if (result.Succeeded)
                {
                    var adminClaim = new Claim("Moderator", moderatorName);
                    userManager.AddClaimAsync(user, adminClaim);
                }
            }
        }


    }
}
