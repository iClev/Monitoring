namespace LPCR.Monitor.JobRunner.Configuration;

public class SchedulerOptions
{
    /// <summary>
    /// Gets or sets the maximum number of concurent jobs the scheduler can handle.
    /// </summary>
    public int MaximumConcurrentJobs { get; set; }
}