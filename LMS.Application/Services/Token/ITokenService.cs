using LMS.Application.DTO.Response.Account;
using LMS.Domain.Entities;

namespace LMS.Application.Services.Token
{
    public interface ITokenService
    {
        Task<AuthResponseDto> GenerateJwtToken(ApplicationUser user);
    }
}
