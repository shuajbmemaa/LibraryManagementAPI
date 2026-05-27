using LMS.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Services.BackgroundServices
{
    public class ReadingReminderService : IReadingReminderService
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<ReadingReminderService> _logger;

        public ReadingReminderService(IBookRepository bookRepository, ILogger<ReadingReminderService> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task SendRemindersAsync(CancellationToken cancellationToken)
        {
            var activeBooks = await _bookRepository.GetActiveBooksWithUsersAsync(cancellationToken);

            foreach (var book in activeBooks)
            {
                _logger.LogWarning(
                    "NOTIFICATION SENT: Hey User {UserId}, don't forget to pick back up your book '{BookName}'!",
                    book.UserId, book.Title);
            }
        }
    }
}
