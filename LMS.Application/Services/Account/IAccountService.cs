using LMS.Application.DTO.Request.Account;
using LMS.Application.DTO.Response.Account;
using LMS.Application.Wrappers;

namespace LMS.Application.Services.Account
{
    public interface IAccountService
    {
        Task<BaseResponse<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<BaseResponse<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
