using Quartz;
using System;
using System.Threading.Tasks;

public class HelloWorldJob : IJob
{
    public Task Execute(IJobExecutionContext context)
    {
        Console.WriteLine($"Hello World! Job ID: {context.JobDetail.Key.Name}, Time: {DateTime.Now}");
        return Task.CompletedTask;
    }
}
