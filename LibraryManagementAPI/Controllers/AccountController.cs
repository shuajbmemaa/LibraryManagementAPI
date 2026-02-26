using LMS.Application.DTO.Request.Account;
using LMS.Application.Services.Account;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _accountService.LoginAsync(dto);
            return CreateResponse(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _accountService.RegisterAsync(dto);
            return CreateResponse(result);
        }
    }
}
