using LPCR.Monitor.JobRunner.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner.Caching;

/// <summary>
/// Provides a mechanism to initialize and run a job processor.
/// </summary>
[DebuggerDisplay("{Name} (job = {JobName})")]
internal sealed class JobProcessor
{
    private readonly Type _jobType;
    private readonly Type _jobProcessorType;

    /// <summary>
    /// Gets the job processor name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the job name.
    /// </summary>
    public string JobName { get; }

    public JobProcessor(Type jobProcessorType, Type jobType)
    {
        _jobProcessorType = jobProcessorType;
        _jobType = jobType;
        Name = jobProcessorType.FullName;
        JobName = jobType.Name;
    }

    /// <summary>
    /// Configures a job processor service provider.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">Configuration builder.</param>
    /// <returns>Service provider for the current job processor instance.</returns>
    public IServiceProvider ConfigureServices(IServiceCollection services, IConfigurationBuilder configuration)
    {
        object jobProcessor = Activator.CreateInstance(_jobProcessorType);
        var method = jobProcessor.GetType().GetMethod("Configure");

        method.Invoke(jobProcessor, new object[] { services, configuration });

        services.AddSingleton(_ => configuration);
        
        return services.BuildServiceProvider();
    }

    /// <summary>
    /// Executes a new instance of the current job processor.
    /// </summary>
    /// <param name="serviceProvider">Job service provider.</param>
    /// <param name="parameters">Job parameters.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns></returns>
    public async Task ExecuteAsync(IServiceProvider serviceProvider, object parameters, CancellationToken cancellationToken)
    {
        ILogger<JobProcessor> logger = serviceProvider.GetService<ILogger<JobProcessor>>();

        try
        {
            IJob job = ActivatorUtilities.CreateInstance(serviceProvider, _jobType) as IJob;

            await job.ExecuteAsync(parameters, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Job '{JobName}' execution failed.");
            throw;
        }
    }
}
