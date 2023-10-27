using LPCR.Monitor.Core;
using LPCR.Monitor.Core.DTO;
using LPCR.Monitor.Core.Http;
using LPCR.Monitor.JobRunner.Caching;
using LPCR.Monitor.JobRunner.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner.Services;

internal class JobExecutor : IJob
{
    private readonly ILogger<JobExecutor> _logger;
    private readonly IMonitoringApi _monitoringApi;
    private readonly JobProcessorCache _jobProcessorCache;

    public JobExecutor(ILogger<JobExecutor> logger, IMonitoringApi monitoringApi, JobProcessorCache jobProcessorCache)
    {
        _logger = logger;
        _monitoringApi = monitoringApi;
        _jobProcessorCache = jobProcessorCache;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        Guid jobId = context.JobDetail.JobDataMap.GetGuid(JobConstants.JobId);

        if (jobId == Guid.Empty)
        {
            throw new InvalidOperationException("Unknown job id.");
        }

        string jobProcessorName = context.JobDetail.JobDataMap.Get(JobConstants.JobProcessor).ToString();
        Guid? jobRunId = await GetJobRunIdAsync(context, jobId);

        if (jobRunId.HasValue)
        {
            try
            {
                JobProcessor job = _jobProcessorCache.GetJobProcessor(jobProcessorName);

                if (job is null)
                {
                    throw new ArgumentException($"Failed to find job processor with name '{jobProcessorName}'.");
                }

                await RunJobAsync(job, jobRunId.Value, context.CancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occured while running job process '{jobRunId.Value}' for job '{jobId}'");

                await _monitoringApi.UpdateJobRunStatusAsync(jobRunId.Value, JobStatusType.Errored);
            }
        }
    }

    private async Task RunJobAsync(JobProcessor job, Guid jobRunId, CancellationToken cancellationToken)
    {
        ServiceCollection services = new();
        ConfigurationBuilder configurationBuilder = new();

        services.AddLogging(x =>
        {
            x.AddJobRunLogger(jobRunId, _monitoringApi);
        });

        IServiceProvider serviceProvider = job.ConfigureServices(services, configurationBuilder);
        ILogger<JobExecutor> jobLogger = serviceProvider.GetRequiredService<ILogger<JobExecutor>>();

        jobLogger.LogInformation($"Starting job '{job.Name}'...");
        await _monitoringApi.UpdateJobRunStatusAsync(jobRunId, JobStatusType.Running);

        try
        {
            await job.ExecuteAsync(serviceProvider, null, cancellationToken);
            await _monitoringApi.UpdateJobRunStatusAsync(jobRunId, JobStatusType.Completed);

            jobLogger.LogInformation($"Job '{job.Name}' completed.");
        }
        catch (Exception ex)
        {
            jobLogger.LogError(ex, $"Job '{job.Name}' execution failed.");

            await _monitoringApi.UpdateJobRunStatusAsync(jobRunId, JobStatusType.Errored);
        }
    }

    private async Task<Guid?> GetJobRunIdAsync(IJobExecutionContext context, Guid jobId)
    {
        Guid jobRunId = context.JobDetail.JobDataMap.GetGuid(JobConstants.JobRunId);

        if (jobRunId == Guid.Empty)
        {
            JobRunDto newJobRun = await _monitoringApi.CreateJobRunAsync(jobId);

            if (newJobRun is null)
            {
                _logger.LogWarning($"Failed to create job process for job: {jobId}");

                return null;
            }

            jobRunId = newJobRun.Id;
        }

        return jobRunId;
    }
}