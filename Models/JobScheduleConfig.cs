namespace JobScheduler.Models;

public class JobScheduleConfig
{
    public int Minute { get; set; }
    public int Hour { get; set; }    
    public string? DayOfWeek { get; set; } 
}

