using LPCR.Monitor.Core;
using LPCR.Monitor.Web.Infrastructure.Persistance;
using LPCR.Monitor.Web.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Pages;

public partial class JobRun
{
    [Inject]
    private IMonitoringDatabase Database { get; set; }

    [Parameter]
    public Guid JobId { get; set; }

    [Parameter]
    public Guid JobRunId { get; set; }

    public bool JobRunNotFound { get; private set; }

    public string JobName { get; private set; }

    public JobRunModel Run { get; private set; }

    public IList<JobRunLogModel> Logs { get; private set; }

    protected override async Task OnInitializedAsync()
    {
        JobName = Database.Jobs.FirstOrDefault(x => x.Id == JobId)?.Name;

        if (string.IsNullOrEmpty(JobName))
        {
            JobRunNotFound = true;
            return;
        }

        Run = await Database.JobRuns
            .Include(x => x.Job)
            .Where(x => x.Id == JobRunId && x.JobId == JobId)
            .Select(x => new JobRunModel
            {
                Id = x.Id,
                JobId = JobId,
                Status = (JobStatusType)x.StatusId,
                Created = x.Created,
                Started = x.Started,
                Completed = x.Completed
            })
            .FirstOrDefaultAsync();

        if (Run is null)
        {
            JobRunNotFound = true;
            return;
        }

        Logs = await Database.JobRunLogs
            .Where(x => x.JobRunId == JobRunId)
            .OrderBy(x => x.Date)
            .Select(x => new JobRunLogModel
            {
                Id = x.Id,
                LogType = (LogType)x.LogTypeId,
                Date = x.Date,
                Message = x.Message,
                Exception = x.Exception
            })
            .ToListAsync();
    }
}
