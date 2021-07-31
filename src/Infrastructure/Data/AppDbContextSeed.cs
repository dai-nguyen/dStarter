using Infrastructure.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class AppDbContextSeed
    {
        public static async Task SeedAsync(
            AppDbContext dbContext,
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            ILoggerFactory loggerFac,
            int? retry = 0)
        {
            int retryForAvail = retry.Value;
            try
            {
                if (!dbContext.Roles.Any())
                {
                    foreach (var role in GetPreconfiguredRoles())
                    {
                        await roleManager.CreateAsync(role);
                    }
                }

                if (!dbContext.Users.Any())
                {
                    var defaultUser = new AppUser()
                    {
                        UserName = "admin",
                        Email = "admin@dstarter.com",
                    };
                    await userManager.CreateAsync(defaultUser, "Pa$$w0rd");
                    await userManager.AddToRoleAsync(defaultUser, "Admin");
                }
            }
            catch (Exception ex)
            {
                if (retryForAvail < 10)
                {
                    retryForAvail++;
                    var log = loggerFac.CreateLogger<AppDbContext>();
                    log.LogError(ex.Message);
                    await SeedAsync(
                        dbContext, 
                        userManager,
                        roleManager,
                        loggerFac, 
                        retryForAvail);
                }
                throw;
            }
        }

        static IEnumerable<AppRole> GetPreconfiguredRoles()
        {
            return new AppRole[]
            {
                new AppRole()
                {
                    Id = "Admin",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    CustomAttributes = new List<CustomAttribute>()
                    {
                        new CustomAttribute("can_add_role", true.ToString()),
                        new CustomAttribute("can_update_role", true.ToString()),
                        new CustomAttribute("can_add_user", true.ToString()),
                        new CustomAttribute("can_update_user", true.ToString())
                    }
                },
                new AppRole()
                {
                    Id = "User",
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
        }
    }
}
