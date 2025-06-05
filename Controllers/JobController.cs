using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class JobController : ControllerBase
{
    private readonly JobSchedulerService _jobService;

    public JobController(JobSchedulerService jobService)
    {
        _jobService = jobService;
    }

    [HttpPost("{type}")]
    public async Task<IActionResult> Schedule(string type, [FromBody] JobScheduleConfig config)
    {
        try
        {
            var jobId = await _jobService.ScheduleJobAsync(type, config);
            return Ok(new { message = "Job scheduled", jobId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}

