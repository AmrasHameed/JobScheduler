using JobScheduler.Models;
using System.Collections.Concurrent;

namespace JobScheduler.Services
{
    public class JobSchedulerService
    {
        private readonly ConcurrentBag<(string Type, JobScheduleConfig Config)> _jobs = new();
        private readonly ConcurrentDictionary<string, DateTime> _lastRunTimes = new();

        public void ScheduleJob(string type, JobScheduleConfig config)
        {
            ValidateConfig(type.ToLower(), config);
            _jobs.Add((type.ToLower(), config));
        }

        public IEnumerable<(string Type, JobScheduleConfig Config)> GetJobs() => _jobs;

        public bool ShouldRun(string jobKey)
        {
            if (_lastRunTimes.TryGetValue(jobKey, out var lastRun))
            {
                var now = DateTime.Now;
                if (lastRun.Minute == now.Minute && lastRun.Hour == now.Hour && lastRun.Date == now.Date)
                {
                    return false;
                }
            }

            _lastRunTimes[jobKey] = DateTime.Now;
            return true;
        }

        private void ValidateConfig(string type, JobScheduleConfig config)
        {
            if (config.Minute < 0 || config.Minute > 59)
                throw new ArgumentException("Minute must be between 0 and 59");

            if (type == "daily" || type == "weekly")
            {
                if (config.Hour < 0 || config.Hour > 23)
                    throw new ArgumentException("Hour must be between 0 and 23");
            }

            if (type == "weekly")
            {
                var validDays = new[] { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
                if (string.IsNullOrWhiteSpace(config.DayOfWeek) || !validDays.Contains(config.DayOfWeek.ToUpper()))
                    throw new ArgumentException("Invalid DayOfWeek");
            }
        }
    }
}
