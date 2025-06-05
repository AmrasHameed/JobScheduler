using Quartz;

public class JobSchedulerService
{
    private readonly ISchedulerFactory _schedulerFactory;

    public JobSchedulerService(ISchedulerFactory schedulerFactory)
    {
        _schedulerFactory = schedulerFactory;
    }

    public async Task<string> ScheduleJobAsync(string type, JobScheduleConfig config)
    {
        var scheduler = await _schedulerFactory.GetScheduler();
        string jobId = Guid.NewGuid().ToString();

        var job = JobBuilder.Create<HelloWorldJob>()
            .WithIdentity(jobId)
            .Build();

        // Validate inputs here (optional, recommended)
        ValidateConfig(type, config);

        string cronExpression = type.ToLower() switch
        {
            "hourly" => $"0 {config.Minute} * ? * *",
            "daily" => $"0 {config.Minute} {config.Hour} ? * *",
            "weekly" => $"0 {config.Minute} {config.Hour} ? * {config.DayOfWeek?.ToUpper()}",
            _ => throw new ArgumentException("Invalid schedule type")
        };

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"{jobId}-trigger")
            .WithCronSchedule(cronExpression)
            .Build();

        await scheduler.ScheduleJob(job, trigger);
        return jobId;
    }

    private void ValidateConfig(string type, JobScheduleConfig config)
    {
        if (config.Minute < 0 || config.Minute > 59)
            throw new ArgumentException("Minute must be between 0 and 59");

        if (type.ToLower() == "daily" || type.ToLower() == "weekly")
        {
            if (config.Hour < 0 || config.Hour > 23)
                throw new ArgumentException("Hour must be between 0 and 23");
        }

        if (type.ToLower() == "weekly")
        {
            var validDays = new HashSet<string> { "SUN", "MON", "TUE", "WED", "THU", "FRI", "SAT" };
            if (string.IsNullOrEmpty(config.DayOfWeek) || !validDays.Contains(config.DayOfWeek.ToUpper()))
                throw new ArgumentException("DayOfWeek must be one of SUN, MON, TUE, WED, THU, FRI, SAT");
        }
    }
}
