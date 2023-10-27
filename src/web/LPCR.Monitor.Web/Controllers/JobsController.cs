using LPCR.Monitor.Core;
using LPCR.Monitor.Core.DTO;
using LPCR.Monitor.Web.Infrastructure.Persistance;
using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JobsController : ControllerBase
{
    private readonly IMonitoringDatabase _database;

    public JobsController(IMonitoringDatabase database)
    {
        _database = database;
    }

    [HttpGet]
    public IActionResult GetJobs()
    {
        IEnumerable<JobDto> jobs = _database.Jobs
            .Select(x => new JobDto
            {
                Id = x.Id,
                Name = x.Name,
                ProcessorName = x.ProcessorName,
                Description = x.Description,
                IsActive = x.IsActive,
                Schedule = x.Schedule,
            });

        return Ok(jobs);
    }

    [HttpGet("{jobId}", Name = "GetJob")]
    public IActionResult GetJob(Guid jobId)
    {
        JobDto job = _database.Jobs
            .Where(x => x.Id == jobId)
            .Select(x => new JobDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ProcessorName = x.ProcessorName,
                IsActive = x.IsActive,
                Schedule = x.Schedule,
            })
            .FirstOrDefault();

        if (job is null)
        {
            return NotFound(nameof(job));
        }

        return Ok(job);
    }

    [HttpPost]
    public async Task<IActionResult> CreateJobAsync([FromBody] CreateJobDto job)
    {
        if (_database.Jobs.Any(x => x.Name.ToLower() == job.Name.ToLower()))
        {
            return Conflict(nameof(job.Name));
        }

        JobEntity newJob = new()
        {
            Name = job.Name,
            Description  = job.Description,
            ProcessorName = job.ProcessorName,
            IsActive = job.IsActive,
        };

        _database.Jobs.Add(newJob);
        await _database.SaveChangesAsync();

        return CreatedAtRoute("GetJob",
            new { jobId = newJob.Id },
            new JobDto
            {
                Id = newJob.Id,
                Name = newJob.Name,
                Description = newJob.Description,
                ProcessorName = newJob.ProcessorName,
            });
    }

    [HttpPost("{jobId}/runs")]
    public async Task<IActionResult> CreateJobRunAsync(Guid jobId)
    {
        if (!_database.Jobs.Any(x => x.Id == jobId))
        {
            return NotFound("job");
        }

        JobRunEntity jobRun = new()
        {
            JobId = jobId,
            StatusId = (int)JobStatusType.Pending
        };

        await _database.JobRuns.AddAsync(jobRun);
        await _database.SaveChangesAsync();

        JobRunDto jobProcessDto = new()
        {
            Id = jobRun.Id,
            JobId = jobRun.JobId,
            Created = jobRun.Created,
            Started = jobRun.Started,
            Completed = jobRun.Completed,
            Status = (JobStatusType)jobRun.StatusId
        };

        return Ok(jobProcessDto);
    }

    [HttpGet("pending")]
    public IActionResult GetPendingJobsAsync()
    {
        IEnumerable<JobRunDto> pendingJobs = _database.JobRuns
            .Where(x => x.StatusId == (int)JobStatusType.Pending)
            .OrderBy(x => x.Created)
            .Select(x => new JobRunDto
            {
                Id = x.Id,
                JobId = x.JobId,
                Created = x.Created,
                Status = (JobStatusType)x.StatusId
            });

        return Ok(pendingJobs);
    }
}
