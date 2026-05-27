using LMS.Application.Services.Account;
using LMS.Application.Services.AI;
using LMS.Application.Services.BackgroundServices;
using LMS.Application.Services.Book;
using LMS.Application.Services.Token;
using LMS.Application.Services.User;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMS.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddHttpClient<IOllamaQueryService, OllamaQueryService>();
            services.AddScoped<IReadingReminderService, ReadingReminderService>();

            return services;
        }
    }
}
