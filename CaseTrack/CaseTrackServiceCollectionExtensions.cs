namespace CaseTrack;

public static class CaseTrackServiceCollectionExtensions
{
    /// <summary>
    /// Registers all CaseTrack-specific dependencies with the dependency injection collection.
    /// </summary>
    /// <param name="services">Service collection to update.</param>
    /// <returns>Updated service collection.</returns>
    public static IServiceCollection AddCaseTrackServices(this IServiceCollection services)
    {
        return services;
    }
}