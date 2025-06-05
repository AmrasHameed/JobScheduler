using Microsoft.OpenApi.Models;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    q.AddJob<HelloWorldJob>(opts => opts
    .WithIdentity("HelloWorldJob")
    .StoreDurably());

});

builder.Services.AddQuartzHostedService(opt => opt.WaitForJobsToComplete = true);
builder.Services.AddSingleton<JobSchedulerService>();
builder.Services.AddControllers();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
