using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance;
using LPCR.Monitor.Web.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Pages;

public partial class Job
{
    [Inject]
    private IMonitoringDatabase Database { get; set; }

    [Parameter]
    public Guid? JobId { get; set; }

    public bool NotFound { get; private set; } = false;

    public JobModel CurrentJob { get; set; }

    public IList<JobRunModel> JobRuns { get; set; }

    protected override Task OnInitializedAsync()
    {
        CurrentJob = Database.Jobs
            .Where(x => x.Id == JobId)
            .Select(x => new JobModel
            {
                Name = x.Name,
                Description = x.Description,
                ProcessorName = x.ProcessorName
            })
            .FirstOrDefault();

        if (CurrentJob is null)
        {
            NotFound = true;
        }

        JobRuns = Database.JobRuns
            .Where(x => x.JobId == JobId)
            .OrderByDescending(x => x.Created)
            .Select(x => new JobRunModel
            {
                Id = x.Id,
                JobId = JobId.Value,
                Status = (JobStatusType)x.StatusId,
                Created = x.Created,
                Started = x.Started,
                Completed = x.Completed
            })
            .ToList();

        return Task.CompletedTask;
    }
}