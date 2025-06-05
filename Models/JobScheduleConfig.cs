public class JobScheduleConfig
{
    public int Minute { get; set; }  // 0-59
    public int Hour { get; set; }    // 0-23, for daily and weekly jobs
    public string? DayOfWeek { get; set; } // MON, TUE, WED, etc. for weekly jobs
}
