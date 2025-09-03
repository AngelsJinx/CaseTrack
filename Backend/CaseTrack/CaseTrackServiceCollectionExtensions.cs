using CaseTrack.Data;
using CaseTrack.Modules;
using Microsoft.EntityFrameworkCore;

namespace CaseTrack;

public static class CaseTrackServiceCollectionExtensions
{
    /// <summary>
    /// Registers all CaseTrack-specific dependencies with the dependency injection collection.
    /// </summary>
    /// <param name="services">Service collection to update.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns>Updated service collection.</returns>
    public static IServiceCollection AddCaseTrackServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register the database connection.
        // There's an argument for this being part of the CaseTrack.Data service injection, but I like it in this project as it's the entrypoint.
        var connectionString = configuration.GetConnectionString("CaseTrackConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("No connection string configured");
        }
        services
            .AddDbContext<CaseTrackContext>(options => options.UseNpgsql(connectionString))
            .AddCaseTrackDataServices(configuration);

        // TODO make interface for TaskModule so it can be easily mocked for tests. Register by interface & update usage (TaskController)
        services.AddScoped<TaskModule>();
        
        return services;
    }
}