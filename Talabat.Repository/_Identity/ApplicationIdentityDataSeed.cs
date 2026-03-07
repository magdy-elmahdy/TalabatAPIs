using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Repository._Identity
{
    public class ApplicationIdentityDataSeed
    {
        public static async Task SeedUsersAsync(UserManager<ApplicationUser> _userManager)
        {
            if (!_userManager.Users.Any())
            {
                var user = new ApplicationUser()
                {
                    DisplayName= "Magdy Elamahdy",
                    Email = "elmahdymagdy4@gmail.com",
                    PhoneNumber= "01129556404",
                    UserName="Admin@Yildiz"
                };
                await _userManager.CreateAsync(user, "Yildiz@123");
            }
        }
    }
}
