namespace LPCR.Monitor.Core;

/// <summary>
/// Defines the different job schedule types.
/// </summary>
public enum JobScheduleType
{
    /// <summary>
    /// Determines the job as a scheduled job aiming to be executed at a given frequency.
    /// </summary>
    Scheduled = 1,

    /// <summary>
    /// Determines a job to be executed on demand.
    /// </summary>
    OnDemand
}