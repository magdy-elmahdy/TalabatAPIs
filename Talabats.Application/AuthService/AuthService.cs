using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabats.Application.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _iConfiguration;

        public AuthService(IConfiguration IConfiguration)
        {
            _iConfiguration = IConfiguration;
        }
        public async Task<string> CreateTokenAsync(ApplicationUser user, UserManager<ApplicationUser> userManager)
        {
            // Private Claims
            var privateClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name ,user.DisplayName),
                new Claim(ClaimTypes.Email ,user.Email),
                new Claim(ClaimTypes.MobilePhone , user.PhoneNumber)
            };
            // Roles
            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var role in userRoles) {
                privateClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            // Headers
            var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_iConfiguration["JWT:AuthKey"]));
            // Token 
            var token = new JwtSecurityToken(
                audience: _iConfiguration["JWT:ValidAudience"],
                issuer: _iConfiguration["JWT:ValidIssuer"],
                expires: DateTime.Now.AddDays(double.Parse(_iConfiguration["JWT:DurationInDays"])),
                claims:privateClaims,
                signingCredentials:new SigningCredentials(AuthKey,SecurityAlgorithms.HmacSha256Signature )
             );
            // Applay Token
            return  new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
