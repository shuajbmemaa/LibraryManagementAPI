namespace LMS.Application.Services.BackgroundServices
{
    public interface IReadingReminderService
    {
        Task SendRemindersAsync(CancellationToken cancellationToken);
    }
}
