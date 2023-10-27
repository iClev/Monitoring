using LPCR.Monitor.JobRunner.Caching;
using LPCR.Monitor.JobRunner.Configuration;
using LPCR.Monitor.JobRunner.Extensions;
using LPCR.Monitor.JobRunner.Http;
using LPCR.Monitor.JobRunner.Logging.Targets;
using LPCR.Monitor.JobRunner.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LPCR.Monitor.JobRunner;

public static class Program
{
    public static async Task Main()
    {
        Console.Title = "LPCR Job Runner";

        IHost host = new HostBuilder()
            .ConfigureAppConfiguration((host, builder) =>
            {
                string environment = Environment.GetEnvironmentVariable("APP_ENVIRONMENT");

                builder.SetBasePath(Environment.CurrentDirectory);
                builder.AddJsonFile("appsettings.json", optional: false);

                if (!string.IsNullOrEmpty(environment))
                {
                    builder.AddJsonFile($"appsettings.{environment}.json", optional: true);
                }
            })
            .ConfigureLogging((host, builder) =>
            {
                ConfigurationItemFactory.Default.Targets.RegisterDefinition("SystemLogging", typeof(SystemLoggingTarget));

                builder.ClearProviders()
                    .AddConfiguration(host.Configuration.GetSection("Logging"))
                    .AddNLog(host.Configuration);

                builder.AddConsole();
            })
            .ConfigureServices((host, services) =>
            {
                services.AddOptions();
                services.Configure<ApplicationOptions>(host.Configuration.GetSection("Application"));
                services.ConfigureScheduler(host.Configuration)
                    .AddHostedService<JobScheduler>()
                    .AddSingleton<JobProcessorCache>(serviceProvider =>
                    {
                        JobProcessorCache cache = ActivatorUtilities.CreateInstance<JobProcessorCache>(serviceProvider);
                        cache.Load();

                        return cache;
                    })
                    .AddMonitoringApi(host.Configuration.GetSection("Application").Get<ApplicationOptions>());
            })
            .Build();

        await host.RunAsync();
    }
}
