using LPCR.Monitor.JobRunner.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;

namespace LPCR.Monitor.JobRunner.Extensions;

/// <summary>
/// Provides extensions to configure the quartz scheduler.
/// </summary>
internal static class QuartzSchedulerExtensions
{
    /// <summary>
    /// Configures the scheduler.
    /// </summary>
    /// <param name="services">Current service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns>Current service collection to chain calls.</returns>
    /// <exception cref="ArgumentException">Thrown when the application option is null.</exception>
    public static IServiceCollection ConfigureScheduler(this IServiceCollection services, IConfiguration configuration)
    {
        var appOptions = configuration.GetSection("Application").Get<ApplicationOptions>();

        if (appOptions is null)
        {
            throw new ArgumentException("Failed to load application options from settings.");
        }

        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();
            q.MaxBatchSize = appOptions.Scheduler.MaximumConcurrentJobs;
            q.InterruptJobsOnShutdown = true;
            q.InterruptJobsOnShutdownWithWait = true;
        });
        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        return services;
    }
}