using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace LPCR.Monitor.Web.Infrastructure.Logging;

/// <summary>
/// Provides extensions for logging.
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configures the logger using NLog.
    /// </summary>
    /// <param name="builder">Application builder.</param>
    /// <returns>Current application builder.</returns>
    public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddConsole();
        }

        return builder;
    }
}
