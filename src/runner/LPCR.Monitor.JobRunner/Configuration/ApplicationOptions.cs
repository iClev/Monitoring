namespace LPCR.Monitor.JobRunner.Configuration;

/// <summary>
/// Provides options to configure the runner application.
/// </summary>
public class ApplicationOptions
{
    /// <summary>
    /// Gets or sets the job mananger API url.
    /// </summary>
    public string ApiUrl { get; set; }

    /// <summary>
    /// Gets or sets the jobs folder.
    /// </summary>
    public string JobsFolder { get; set; }

    /// <summary>
    /// Gets or sets the worker refresh rate in seconds.
    /// </summary>
    public double RefreshRateInSeconds { get; set; }

    /// <summary>
    /// Gets or sets the scheduler options.
    /// </summary>
    public SchedulerOptions Scheduler { get; set; }
}
