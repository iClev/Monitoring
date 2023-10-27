namespace LPCR.Monitor.Core;

/// <summary>
/// Defines the different types of traces.
/// </summary>
public enum LogType
{
    /// <summary>
    /// Logs that track the general flow of the application. These logs should have long-term value.
    /// </summary>
    Information = 1,

    /// <summary>
    /// Logs that highlight an abnormal or unexpected event in the application flow, but do not otherwise cause the application execution to stop.*
    /// </summary>
    Warning,

    /// <summary>
    /// Logs that highlight when the current flow of execution is stopped due to a failure.
    /// These should indicate a failure in the current activity, not an application-wide failure.
    /// </summary>
    Error,

    /// <summary>
    /// Logs that describe an unrecoverable application or system crash, or a catastrophic failure that requires immediate attention.
    /// </summary>
    Critical,
}
