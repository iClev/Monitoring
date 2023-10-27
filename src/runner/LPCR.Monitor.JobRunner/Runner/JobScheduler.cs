using LPCR.Monitor.Core.DTO;
using LPCR.Monitor.Core.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner.Services;

/// <summary>
/// Provides a mechanism to schedule and execute the jobs.
/// </summary>
internal class JobScheduler : BackgroundService
{
    private readonly ILogger<JobScheduler> _logger;
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IMonitoringApi _monitoringApi;

    private IScheduler _scheduler;

    public JobScheduler(ILogger<JobScheduler> logger, ISchedulerFactory schedulerFactory, IMonitoringApi monitoringApi)
	{
        _logger = logger;
        _schedulerFactory = schedulerFactory;
        _monitoringApi = monitoringApi;
    }

    /// <summary>
    /// Executes the scheduler background service.
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting runner background service.");

        _scheduler = await _schedulerFactory.GetScheduler(stoppingToken);

        IEnumerable<JobDto> jobs = await _monitoringApi.GetJobsAsync();

        foreach (JobDto job in jobs)
        {
            await ScheduleJobAsync(job);
        }
    }

    /// <summary>
    /// Stops the scheduler background service.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping runner background service.");

        await _scheduler.Shutdown();
    }

    /// <summary>
    /// Schedule a job.
    /// </summary>
    /// <param name="job">Job to schedule.</param>
    /// <returns></returns>
    private async Task ScheduleJobAsync(JobDto job)
    {
        IJobDetail jobDetail = JobBuilder.Create<JobExecutor>()
            .WithIdentity(job.ProcessorName, job.ProcessorName)
            .UsingJobData(new()
            {
                { JobConstants.JobId, job.Id },
                { JobConstants.JobRunId, Guid.Empty },
                { JobConstants.JobProcessor, job.ProcessorName }
            })
            .Build();

        TriggerBuilder builder = TriggerBuilder.Create();

        if (!string.IsNullOrWhiteSpace(job.Schedule))
        {
            builder = builder
                .WithCronSchedule(job.Schedule)
                .StartNow();
        }

        ITrigger trigger = builder.Build();

        await _scheduler.ScheduleJob(jobDetail, trigger);
    }
}
