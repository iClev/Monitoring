using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LPCR.Monitor.JobRunner.Abstractions;

/// <summary>
/// Provides a mechanism to create a job processor with that will execute the given job as generic parameter.
/// </summary>
/// <typeparam name="TJob">Job type to execute.</typeparam>
public interface IJobProcessor<TJob> where TJob : IJob
{
    /// <summary>
    /// Configure the job execution context.
    /// </summary>
    /// <param name="services">Service collection for dependency injection.</param>
    /// <param name="configuration">Configuration builder.</param>
    void Configure(IServiceCollection services, IConfigurationBuilder configuration);
}