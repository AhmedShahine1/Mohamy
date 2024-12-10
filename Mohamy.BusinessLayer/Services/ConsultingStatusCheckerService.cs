using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mohamy.BusinessLayer.Interfaces;
using Mohamy.Core.Helpers;

namespace Mohamy.BusinessLayer.Services
{
    public class ConsultingStatusCheckerService : IHostedService, IDisposable
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private readonly TimeZoneInfo _saudiTimeZone;

        public ConsultingStatusCheckerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            _saudiTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time"); // Saudi Arabia time zone
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Set the timer to run every hour
            _timer = new Timer(async (state) => await CheckConsultingStatusAsync(), null, TimeSpan.Zero, TimeSpan.FromHours(1));
            return Task.CompletedTask;
        }

        private async Task CheckConsultingStatusAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var scoppedService = scope.ServiceProvider.GetService<IConsultingService>();
                var consultings = await scoppedService.GetAllConsultingsAsync();

                foreach (var consulting in consultings)
                {
                    if (consulting.EndDate.HasValue && consulting.StatusConsulting != statusConsulting.Completed.ToString())
                    {
                        DateTime currentSaudiTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _saudiTimeZone);

                        if (currentSaudiTime >= consulting.EndDate.Value)
                        {
                            await scoppedService.UpdateConsultingStatusAsync(consulting.Id, statusConsulting.Completed);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Stop the timer when the service is stopped
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            // Dispose the timer
            _timer?.Dispose();
        }
    }

}
