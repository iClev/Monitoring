using LPCR.Monitor.Web.Infrastructure.Persistance.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.Web.Infrastructure.Persistance;

/// <summary>
/// Provides an abstraction to query the different tables in the monitoring database.
/// </summary>
public interface IMonitoringDatabase
{
    /// <summary>
    /// Gets the jobs.
    /// </summary>
    DbSet<JobEntity> Jobs { get; }

    /// <summary>
    /// Gets the job runs.
    /// </summary>
    DbSet<JobRunEntity> JobRuns { get; }

    /// <summary>
    /// Gets the job run logs.
    /// </summary>
    DbSet<JobRunLogEntity> JobRunLogs { get; }

    /// <summary>
    /// Gets the job status types.
    /// </summary>
    DbSet<JobStatusTypeEntity> JobStatusTypes { get; }

    /// <summary>
    /// Gets the job schedule types.
    /// </summary>
    DbSet<JobScheduleTypeEntity> JobScheduleTypes { get; }

    /// <summary>
    /// Gets the log types.
    /// </summary>
    DbSet<LogTypeEntity> LogTypes { get; }

    /// <summary>
    /// Gets the system logs.
    /// </summary>
    DbSet<SystemLogEntity> SystemLogs { get; }

    /// <summary>
    /// Save the pending changes.
    /// </summary>
    /// <returns></returns>
    int SaveChanges();

    /// <summary>
    /// Save the pending changes.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
