using JobScheduler.Models;
using JobScheduler.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Controllers;

[ApiController]
[Route("api/job")]
public class JobController : ControllerBase
{
    private readonly JobSchedulerService _jobService;

    public JobController(JobSchedulerService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost("{type}")]
    public IActionResult Schedule(string type, [FromBody] JobScheduleConfig config)
    {
        try
        {
            _jobService.ScheduleJob(type, config);
            return Ok(new { message = $"Job scheduled as {type}" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
