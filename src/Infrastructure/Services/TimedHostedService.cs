using Infrastructure.Entities;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Mappers;
using Infrastructure.Specifications;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class TimedHostedService : IHostedService, IDisposable
    {
        private int _isStarted = 0;
        private readonly ILogger<TimedHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IBackgroundTaskQueue _taskQueue;
        private Timer _timer;

        public TimedHostedService(
            ILogger<TimedHostedService> logger,
            IServiceProvider serviceProvider,
            IBackgroundTaskQueue taskQueue)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _taskQueue = taskQueue;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service running.");

            _timer = new Timer(DoWorkAsync, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWorkAsync(object state)
        {
            _logger.LogInformation(
                $"Timed Hosted Service is working. Started: {_isStarted}");

            // started
            if (0 != Interlocked.Exchange(ref _isStarted, 1))
            {
                return;
            }

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {

                    await Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unable to execute DoWork method");
            }
            finally
            {
                //Release the lock
                Interlocked.Exchange(ref _isStarted, 0);
            }
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Timed Hosted Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
