using LPCR.Monitor.Core.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;

namespace LPCR.Monitor.JobRunner.Logging;

/// <summary>
/// Provides a mechanism to create and provide loggers for a given job run.
/// </summary>
internal sealed class JobLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, JobLogger> _loggers = new(StringComparer.OrdinalIgnoreCase);
    private readonly Guid _jobRunId;
    private readonly IMonitoringApi _monitoringApi;

    /// <summary>
    /// Creates a new <see cref="JobLoggerProvider"/> instance.
    /// </summary>
    /// <param name="jobRunId">Job run id.</param>
    /// <param name="monitoringApi">Monitoring API.</param>
    public JobLoggerProvider(Guid jobRunId, IMonitoringApi monitoringApi)
    {
        _jobRunId = jobRunId;
        _monitoringApi = monitoringApi;
    }

    /// <summary>
    /// Creates a logger for a given category name.
    /// </summary>
    /// <param name="categoryName">Logger category name.</param>
    /// <returns>The logger.</returns>
    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new JobLogger(_jobRunId, _monitoringApi));
    }

    /// <summary>
    /// Releases the logger provider resources.
    /// </summary>
    public void Dispose()
    {
        _loggers.Clear();
    }
}
