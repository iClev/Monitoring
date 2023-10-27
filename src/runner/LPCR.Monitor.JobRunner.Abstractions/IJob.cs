using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner.Abstractions;

/// <summary>
/// Provides a mechanism to create a job.
/// </summary>
public interface IJob
{
    /// <summary>
    /// Starts the job with the given parameters.
    /// </summary>
    /// <param name="parameters">Job parameters. (may be null)</param>
    /// <param name="cancellationToken">Task cancellation token.</param>
    /// <returns></returns>
    Task ExecuteAsync(object parameters, CancellationToken cancellationToken);
}
