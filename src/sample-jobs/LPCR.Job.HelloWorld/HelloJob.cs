using LPCR.Monitor.JobRunner.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LPCR.Job.HelloWorld;

public class HelloJobProcessor : IJobProcessor<HelloJob>
{
    public void Configure(IServiceCollection services, IConfigurationBuilder configuration)
    {
        services.AddSingleton<Service>();
    }
}

public class HelloJob : IJob
{
    private readonly Service _service;
    private readonly ILogger<HelloJob> _logger;

    public HelloJob(Service service, ILogger<HelloJob> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task ExecuteAsync(object parameters, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Hello world! - from LPCR.Job.HelloWorld job :-)");
        for (int i = 0; i < 10; i++)
        {
            _logger.LogWarning($"Hello {i + 1}!");
            await Task.Delay(1000);
        }
        _logger.LogInformation($"Hello world over!");
    }
}

public class Service
{
    public void Display(string text) => Console.WriteLine(text);
}