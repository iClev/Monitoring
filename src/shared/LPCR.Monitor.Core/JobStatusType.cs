namespace LPCR.Monitor.Core;

/// <summary>
/// Define the job status types.
/// </summary>
public enum JobStatusType
{
    /// <summary>
    /// Unknown job status type.
    /// </summary>
    Unknown = 1,

    /// <summary>
    /// Indicates that the job is pendning.
    /// </summary>
    Pending,

    /// <summary>
    /// Indicates that the job is queued by the runner.
    /// </summary>
    Queued,

    /// <summary>
    /// Indicates that the job is being executed by the runner.
    /// </summary>
    Running,


    /// <summary>
    /// Indicates that the job is completed without errors.
    /// </summary>
    Completed,

    /// <summary>
    /// Indicates that the job has encountered an error.
    /// </summary>
    Errored,

    /// <summary>
    /// Indicates that the job has been canceled by an external action or user.
    /// </summary>
    Canceled
}
