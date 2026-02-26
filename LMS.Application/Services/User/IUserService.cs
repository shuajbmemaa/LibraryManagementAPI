using LMS.Application.DTO.Request.User;
using LMS.Application.DTO.Response.User;
using LMS.Application.Wrappers;

namespace LMS.Application.Services.User
{
    public interface IUserService
    {
        Task<BaseResponse<string>> CreateAsync(CreateUserDto dto);
        Task<BaseResponse<List<UserResponseDto>>> GetAllAsync();
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
    }
}
