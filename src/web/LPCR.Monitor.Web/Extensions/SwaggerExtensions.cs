using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LPCR.Monitor.Web.Extensions;

/// <summary>
/// Provides extensions to configure and manage Swagger.
/// </summary>
public static class SwaggerExtensions
{
    /// <summary>
    /// Adds swagger to the given service collection.
    /// </summary>
    /// <param name="services">Application service collection.</param>
    /// <returns>Current service collection.</returns>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    /// <summary>
    /// Tells the application that it should use swagger and swagger UI.
    /// </summary>
    /// <param name="app">Applicaction builder.</param>
    /// <returns>Current application builder.</returns>
    public static IApplicationBuilder UseSwaggerInterface(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}