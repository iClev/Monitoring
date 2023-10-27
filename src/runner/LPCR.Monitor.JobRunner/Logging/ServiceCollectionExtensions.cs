using LPCR.Monitor.Core.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LPCR.Monitor.JobRunner.Logging;

/// <summary>
/// Provides extensions to manage the loggers.
/// </summary>
internal static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the job run logger to the current logging builder.
    /// </summary>
    /// <param name="builder">Logging builder.</param>
    /// <param name="jobRunId">Job run id.</param>
    /// <param name="monitoringApi">Monitoring API.</param>
    /// <returns>Logging builder.</returns>
    public static ILoggingBuilder AddJobRunLogger(this ILoggingBuilder builder, Guid jobRunId, IMonitoringApi monitoringApi)
    {
        builder.Services.TryAddEnumerable(
            ServiceDescriptor.Singleton<ILoggerProvider, JobLoggerProvider>(serviceProvider =>
            {
                return new JobLoggerProvider(jobRunId, monitoringApi);
            })
        );

        return builder;
    }
}