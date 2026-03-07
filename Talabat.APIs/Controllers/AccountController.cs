using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        public UserManager<ApplicationUser> _UserManager { get; }
        public SignInManager<ApplicationUser> _SignInManager { get; }
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager1)
        {
            _UserManager = userManager;
            _SignInManager = signInManager1;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _UserManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var pass = await _SignInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!pass.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = "This Will Be Token Soon"

            });
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.Phone
            };
            var result =  await _UserManager.CreateAsync(user, model.Password);
            //if (!result.Succeeded) return BadRequest(new ApiValidationErrorResponse() { Errors = result.Errors.Select(E => E.Description) });
            if (!result.Succeeded) return BadRequest(400);

            return Ok(new UserDto()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                Token = "This Will Be Token"
            });
        }

    }
}
