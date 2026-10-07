using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;

namespace Talabat.APIs.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IMapper _mapper;

        public UserManager<ApplicationUser> _userManager { get; }
        public SignInManager<ApplicationUser> _SignInManager { get; }
        public IAuthService _authService { get; }

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager1,
            IAuthService authService,
            IMapper mapper)
        {
            _userManager = userManager;
            _SignInManager = signInManager1;
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            var pass = await _SignInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!pass.Succeeded) return Unauthorized(new ApiResponse(401, "Invalid Login"));

            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)

            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PnoneNumber
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(400);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);
            return Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = await _authService.CreateTokenAsync(user, _userManager)
            });
        }

        [Authorize(AuthenticationSchemes = ("Bearer"))]
        [HttpGet("address")]
        public async Task<ActionResult<Address>> GetCurrentUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users.Include(U => U.Address).FirstOrDefaultAsync(U => U.Email == email);
            return Ok(_mapper.Map<AddressDto>(user.Address));

        }


        /// Update the address of the current user
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto addressDto)
        
        {
            //var updatedAddress = _mapper.Map<Address>(addressDto);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users
                .Include(u => u.Address)
                .FirstOrDefaultAsync(u => u.Email == email);
            //updatedAddress.Id =user.Address.Id;
            //user.Address = updatedAddress;
            //_mapper.Map(addressDto, user.Address);

            // Update the existing Address
            _mapper.Map(addressDto, user.Address);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(
                    new ApiResponse(400, "Problem updating the user address")
                );
            }

            return Ok(addressDto);
        }
    }
}
