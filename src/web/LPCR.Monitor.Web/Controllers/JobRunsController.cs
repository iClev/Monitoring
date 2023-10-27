using LPCR.Monitor.Core;
using LPCR.Monitor.Core.DTO;
using LPCR.Monitor.Web.Infrastructure.Persistance;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobRunsController : ControllerBase
{
    private readonly IMonitoringDatabase _database;

    public JobRunsController(IMonitoringDatabase database)
    {
        _database = database;
    }

    [HttpPost("{jobRunId}/{newStatus}")]
    public async Task<IActionResult> UpdateJobProcessStatusAsync(Guid jobRunId, JobStatusType newStatus)
    {
        JobRunEntity jobRun = _database.JobRuns.FirstOrDefault(x => x.Id == jobRunId);

        if (jobRun is null)
        {
            return NotFound(nameof(jobRun));
        }

        var oldStatus = (JobStatusType)jobRun.StatusId;

        if (oldStatus < newStatus)
        {
            if (newStatus == JobStatusType.Running)
            {
                jobRun.Started = DateTime.UtcNow;
            }

            if (newStatus == JobStatusType.Completed || newStatus == JobStatusType.Errored)
            {
                jobRun.Completed = DateTime.UtcNow;
            }
        }

        jobRun.StatusId = (int)newStatus;

        await _database.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost("{jobRunId}/logs")]
    public async Task<IActionResult> CreateJobRunLogAsync(Guid jobRunId, [FromBody] CreateJobRunLogDto log)
    {
        JobRunEntity jobRun = _database.JobRuns.FirstOrDefault(x => x.Id == jobRunId);

        if (jobRun is null)
        {
            return NotFound(nameof(jobRun));
        }

        await _database.JobRunLogs.AddAsync(new JobRunLogEntity
        {
            JobRunId = jobRunId,
            Date = log.Date,
            LogTypeId = (int)log.Type,
            Message = log.Message,
            Exception = log.Exception
        });
        await _database.SaveChangesAsync();

        return Ok();
    }
}
