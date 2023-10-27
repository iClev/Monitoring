using LPCR.Monitor.Core;
using LPCR.Monitor.Core.DTO.System;
using LPCR.Monitor.Core.Http;
using NLog;
using NLog.Targets;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner.Logging.Targets;

[Target("SystemLogging")]
public sealed class SystemLoggingTarget : AsyncTaskTarget
{
    private static readonly Dictionary<int, LogType> _logLevels = new()
    {
        { LogLevel.Info.Ordinal, LogType.Information },
        { LogLevel.Warn.Ordinal, LogType.Warning },
        { LogLevel.Error.Ordinal, LogType.Error },
        { LogLevel.Fatal.Ordinal, LogType.Critical }
    };

    private IMonitoringApi _monitoringApi;

    public SystemLoggingTarget()
    {
    }

    protected override void InitializeTarget()
    {
        _monitoringApi = ResolveService<IMonitoringApi>();

        base.InitializeTarget();
    }

    protected override async Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken)
    {
        if (_logLevels.TryGetValue(logEvent.Level.Ordinal, out LogType logType))
        {
            CreateSystemLogDto log = new()
            {
                Type = logType,
                Date = logEvent.TimeStamp,
                Message = logEvent.FormattedMessage,
                Exception = logEvent.Exception?.ToString()
            };

            await _monitoringApi.CreateSystemLogAsync(log);
        }
    }
}
