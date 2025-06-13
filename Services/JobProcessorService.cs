using JobScheduler.Models;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace JobScheduler.Services
{
    public class JobProcessorService : BackgroundService
    {
        private readonly JobSchedulerService _jobService;

        public JobProcessorService(JobSchedulerService jobService)
        {
            _jobService = jobService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Align to next full minute
            var now = DateTime.Now;
            var delayUntilNextMinute = TimeSpan.FromMinutes(1) - TimeSpan.FromSeconds(now.Second) - TimeSpan.FromMilliseconds(now.Millisecond);
            await Task.Delay(delayUntilNextMinute, stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                now = DateTime.Now;

                foreach (var (type, config) in _jobService.GetJobs())
                {
                    switch (type)
                    {
                        case "hourly":
                            if (now.Minute == config.Minute &&
                                _jobService.ShouldRun($"{type}-{config.Minute}"))
                            {
                                RunTask(type, config);
                            }
                            break;

                        case "daily":
                            if (now.Hour == config.Hour &&
                                now.Minute == config.Minute &&
                                _jobService.ShouldRun($"{type}-{config.Hour}-{config.Minute}"))
                            {
                                RunTask(type, config);
                            }
                            break;

                        case "weekly":
                            if (now.Hour == config.Hour &&
                                now.Minute == config.Minute &&
                                now.DayOfWeek.ToString().StartsWith(config.DayOfWeek!, true, CultureInfo.InvariantCulture) &&
                                _jobService.ShouldRun($"{type}-{config.Hour}-{config.Minute}-{config.DayOfWeek}"))
                            {
                                RunTask(type, config);
                            }
                            break;
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private void RunTask(string type, JobScheduleConfig config)
        {
            Console.WriteLine($"[{DateTime.Now}] Executing {type.ToUpper()} job");
        }
    }
}
