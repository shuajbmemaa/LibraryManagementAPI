using LMS.Application.DTO.Request.Account;
using LMS.Application.DTO.Response.Account;
using LMS.Application.Services.Token;
using LMS.Application.Wrappers;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;

        public AccountService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            {
                return BaseResponse<AuthResponseDto>.BadRequest("Invalid Credentials");
            }

            var token = await _tokenService.GenerateJwtToken(user);

            return BaseResponse<AuthResponseDto>.Ok(token);
        }

        public async Task<BaseResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto)
        {
            var user = MapToApplication(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            await _userManager.AddToRoleAsync(user, "User");

            var token = await _tokenService.GenerateJwtToken(user);

            return BaseResponse<AuthResponseDto>.Ok(token);
        }

        private ApplicationUser MapToApplication(RegisterDto dto)
        {
            return new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow
            };
        }
    }
}
