using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;
using MultipleServers.Jobs.Abstract;
using MultipleServers.Server1.Jobs;

namespace MultipleServers.Server1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecurringJobsController: ControllerBase
{
    private readonly IRecurringJobManager _recurringJobManager;

    public RecurringJobsController(
        IRecurringJobManager recurringJobManager)
    {
        _recurringJobManager = recurringJobManager;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRecurringJob([FromBody] RecurringJobInputModel inputModel, CancellationToken cancellationToken)
    {
        _recurringJobManager.AddOrUpdate<RecurringJobExecutor>(
            inputModel.Id,
            x => x.ExecuteJob(typeof(SampleRecurringJobServer1).AssemblyQualifiedName, new {}, cancellationToken),
            inputModel.CronExpression,
            queue: "server1-job1");

        return CreatedAtAction(nameof(Get), inputModel.Id, inputModel.Id);
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var jobs = JobStorage.Current.GetConnection().GetRecurringJobs(new List<string> { id });
        if (jobs.Count <= 0)
            return NotFound();
        return Ok(jobs.FirstOrDefault());
    }

    public record RecurringJobInputModel(string Id, string CronExpression);
}