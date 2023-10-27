using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LPCR.Monitor.Web.Infrastructure.Persistance;

/// <summary>
/// Provides extensions to manage the persistance layer.
/// </summary>
public static class PersistanceExtensions
{
    /// <summary>
    /// Adds the persistance layer to the current service collection.
    /// </summary>
    /// <param name="services">Service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns></returns>
    public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IMonitoringDatabase, MonitoringDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        });

        return services;
    }
}