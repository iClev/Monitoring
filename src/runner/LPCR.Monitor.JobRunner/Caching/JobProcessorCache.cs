using LPCR.Monitor.JobRunner.Abstractions;
using LPCR.Monitor.JobRunner.Configuration;
using LPCR.Monitor.JobRunner.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LPCR.Monitor.JobRunner.Caching;

/// <summary>
/// Provides a mechanism to cache the loaded job processors.
/// </summary>
internal class JobProcessorCache : IDisposable
{
    private readonly Dictionary<string, JobProcessor> _processors = new();
    private readonly IOptions<ApplicationOptions> _applicationOptions;
    private readonly ILogger<JobProcessorCache> _logger;

    public JobProcessorCache(IOptions<ApplicationOptions> applicationOptions, ILogger<JobProcessorCache> logger)
    {
        _applicationOptions = applicationOptions ?? throw new ArgumentNullException(nameof(applicationOptions));
        _logger = logger;
    }

    /// <summary>
    /// Loads all job processors from the jobs folder.
    /// </summary>
    public void Load()
    {
        string jobsFolder = _applicationOptions.Value.JobsFolder ?? "jobs";
        string jobPath = Path.Combine(Environment.CurrentDirectory, jobsFolder);
        IEnumerable<string> directories = Directory.EnumerateDirectories(jobPath);

        foreach (string jobDirectory in directories)
        {
            // Get the processor name from the job directory path.
            // Note: the folder name **should** be the same as the processor name.
            string processorName = Path.GetFileName(jobDirectory);

            JobLoadingContext context = new(jobDirectory);
            Assembly jobAssembly = context.LoadFromAssemblyPath(Path.Combine(jobDirectory, $"{processorName}.dll"));

            IEnumerable<Type> processorTypes = jobAssembly.GetTypes()
                .Where(x => x.ImplementsInterface(typeof(IJobProcessor<>)))
                .ToList();

            foreach (Type jobProcessorType in processorTypes)
            {
                if (jobProcessorType is not null)
                {
                    Type jobType = jobProcessorType.GetInterfaces()
                        .Single(x => x.GetGenericTypeDefinition() == typeof(IJobProcessor<>))
                        .GetGenericArguments()
                        .Single();
                    JobProcessor processor = new(jobProcessorType, jobType);

                    if (!_processors.TryAdd(processor.Name, processor))
                    {
                        _logger.LogError($"Failed to add a job processor with name: '{processor.Name}'.");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Gets the job processor by the given name.
    /// </summary>
    /// <param name="jobProcessorName">Job processor name.</param>
    /// <returns>Job processor if found; null otherwise.</returns>
    public JobProcessor GetJobProcessor(string jobProcessorName)
    {
        return _processors.TryGetValue(jobProcessorName, out JobProcessor processor) ? processor : null;
    }

    /// <summary>
    /// Reload all job processors.
    /// </summary>
    public void Reload()
    {
        _processors.Clear();
        Load();
    }

    /// <summary>
    /// Releases the job processors and clear the cache.
    /// </summary>
    public void Dispose()
    {
        _processors.Clear();
    }
}
