using LMS.Application.DTO.Request.User;
using LMS.Application.DTO.Response.User;
using LMS.Application.Wrappers;
using LMS.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LMS.Application.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<BaseResponse<string>> CreateAsync(CreateUserDto dto)
        {
            if (!await _roleManager.RoleExistsAsync(dto.Role))
            {
                return BaseResponse<string>.BadRequest("Role does not exist");
            }
                
            var user = MapToEntity(dto);

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return BaseResponse<string>.BadRequest(errors);
            }
                
            await _userManager.AddToRoleAsync(user, dto.Role);

            return BaseResponse<string>.Ok($"User created successfully. Id: {user.Id}");
        }

        private ApplicationUser MapToEntity(CreateUserDto dto)
        {
            return new ApplicationUser
            {
                UserName = dto.Email,
                Email = dto.Email,
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return BaseResponse<bool>.NotFound("User not found");
            }

            user.IsDeleted = true;

            await _userManager.UpdateAsync(user);

            return BaseResponse<bool>.Ok(true);
        }

        public async Task<BaseResponse<List<UserResponseDto>>> GetAllAsync()
        {
            var users = _userManager.Users.Where(x => !x.IsDeleted).ToList();
            var list = new List<UserResponseDto>();

            foreach (var user in users)
            {
                list.Add(await MapToDtoAsync(user));
            }

            return BaseResponse<List<UserResponseDto>>.Ok(list);
        }

        private async Task<UserResponseDto> MapToDtoAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return new UserResponseDto
            {
                Id = user.Id,
                Email = user.Email!,
                Name = user.Name,
                Roles = roles
            };
        }
    }
}
