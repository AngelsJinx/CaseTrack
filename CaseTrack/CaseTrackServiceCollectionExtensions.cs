using CaseTrack.Data;
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
        var connectionString = configuration.GetConnectionString("CaseTrackConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new Exception("No connection string configured");
        }

        services.AddDbContext<CaseTrackContext>(options => options.UseNpgsql(connectionString));
        
        return services;
    }
}