using Fotografolio.Data.Models;
using Fotografolio.Data.Interfaces;
using Fotografolio.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fotografolio.Data
{
    public static class ApplicationDbInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            IOptions<AppSettings> appsettings)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, appsettings);
            //SeedOtherTables(unitOfWork);
            //unitOfWork.SaveAsync();
        }
        public static void SeedUsers(UserManager<ApplicationUser> userManager, IOptions<AppSettings> appsettings)
        {
            try
            {
                if (userManager.FindByEmailAsync(appsettings.Value.AdminEmail).GetAwaiter().GetResult() == null)
                {
                    ApplicationUser user = new ApplicationUser
                    {
                        api_token = "some",
                        UserName = appsettings.Value.AdminEmail,
                        Email = appsettings.Value.AdminEmail,
                        EmailConfirmed = true,
                        created_at = DateTime.Now,
                        updated_at = DateTime.Now
                    };

                    IdentityResult result = userManager.CreateAsync(user, appsettings.Value.AdminPassword).GetAwaiter().GetResult();

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Admin").GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

        }
        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            try
            {
                if (!roleManager.RoleExistsAsync("Adnin").GetAwaiter().GetResult())
                {
                    IdentityResult result = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "Admin",
                        NormalizedName = "ADMIN"
                    }).GetAwaiter().GetResult();
                    result = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    }).GetAwaiter().GetResult();
                    result = roleManager.CreateAsync(new IdentityRole
                    {
                        Name = "Guest",
                        NormalizedName = "GUEST"
                    }).GetAwaiter().GetResult();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

        }

        public static bool SeedOtherTables(IUnitOfWork unitOfWork)
        {
            return unitOfWork.CreateDefaults();
        }
    }
}
