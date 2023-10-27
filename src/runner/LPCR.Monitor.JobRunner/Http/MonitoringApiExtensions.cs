using LPCR.Monitor.Core.Http;
using LPCR.Monitor.JobRunner.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LPCR.Monitor.JobRunner.Http;

/// <summary>
/// Provides extensions to manage the Monitoring API interface.
/// </summary>
internal static class MonitoringApiExtensions
{
    private static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() },
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
    };

    /// <summary>
    /// Adds the monitoring API to the given service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="applicationOptions">Application options.</param>
    /// <returns>Current service collection.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the given application options is null.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the monitoring URL is null or empty.</exception>
    public static IServiceCollection AddMonitoringApi(this IServiceCollection services, ApplicationOptions applicationOptions)
    {
        ArgumentNullException.ThrowIfNull(services, nameof(services));

        RefitSettings settings = new(new SystemTextJsonContentSerializer(DefaultJsonSerializerOptions));

        services.AddRefitClient<IMonitoringApi>(settings)
            .ConfigureHttpClient(httpClient =>
            {
                if (string.IsNullOrWhiteSpace(applicationOptions.ApiUrl))
                {
                    throw new InvalidOperationException("The JobManager API url is empty.");
                }

                httpClient.BaseAddress = new Uri(applicationOptions.ApiUrl);
            });

        return services;
    }
}
