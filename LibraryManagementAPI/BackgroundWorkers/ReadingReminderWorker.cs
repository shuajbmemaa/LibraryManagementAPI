using LMS.Application.Services.BackgroundServices;

namespace LibraryManagementAPI.BackgroundWorkers
{
    public class ReadingReminderWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReadingReminderWorker> _logger;

        public ReadingReminderWorker(IServiceProvider serviceProvider, ILogger<ReadingReminderWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Reading Reminder Background Worker is starting.");

            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(150000));

            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Background timer ticked. Initiating reading reminder check...");

                try
                {
                    // CRITICAL: BackgroundService is a Singleton. Because IReadingReminderService 
                    // is registered as Scoped (and relies on a Scoped DbContext), we must manually 
                    // create a dependency injection scope for every tick.
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var reminderService = scope.ServiceProvider.GetRequiredService<IReadingReminderService>();

                        await reminderService.SendRemindersAsync(stoppingToken);
                    }

                    _logger.LogInformation("Reading reminder check completed successfully.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while executing the Reading Reminder background job.");
                }
            }

            _logger.LogInformation("Reading Reminder Background Worker is stopping.");
        }
    }
}
