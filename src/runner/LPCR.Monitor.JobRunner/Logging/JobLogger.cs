using LPCR.Monitor.Core;
using LPCR.Monitor.Core.Http;
using Microsoft.Extensions.Logging;
using System;

namespace LPCR.Monitor.JobRunner.Logging;

/// <summary>
/// Custom job logger.
/// </summary>
internal sealed class JobLogger : ILogger
{
    private readonly Guid _jobRunId;
    private readonly IMonitoringApi _monitoringApi;

    /// <summary>
    /// Creates a new <see cref="JobLogger"/> instance.
    /// </summary>
    /// <param name="jobRunId">Job Run Id.</param>
    /// <param name="monitoringApi">Monitoring API.</param>
    public JobLogger(Guid jobRunId, IMonitoringApi monitoringApi)
    {
        _jobRunId = jobRunId;
        _monitoringApi = monitoringApi;
    }

    /// <inheritdoc />
    public IDisposable BeginScope<TState>(TState state) => default;

    /// <inheritdoc />
    public bool IsEnabled(LogLevel logLevel) => true;

    /// <inheritdoc />
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        Console.WriteLine($"[{DateTime.Now:G}](JobId={_jobRunId})|[{logLevel.ToString().ToUpperInvariant()}]: {formatter(state, exception)}");
        _monitoringApi.CreateJobRunLogAsync(_jobRunId, new Core.DTO.CreateJobRunLogDto
        {
            Type = logLevel switch
            {
                LogLevel.Information => LogType.Information,
                LogLevel.Warning => LogType.Warning,
                LogLevel.Error => LogType.Error,
                LogLevel.Critical => LogType.Critical,
                _ => throw new NotImplementedException()
            },
            Date = DateTime.UtcNow,
            Message = state.ToString(),
            Exception = exception?.ToString()
        }).Wait();
    }
}
