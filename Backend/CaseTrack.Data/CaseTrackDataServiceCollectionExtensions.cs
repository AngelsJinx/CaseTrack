using CaseTrack.Data.Entities;
using CaseTrack.Data.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CaseTrack.Data;

public static class CaseTrackDataServiceCollectionExtensions
{
    public static IServiceCollection AddCaseTrackDataServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IRepository<CaseTrackTask>, TaskRepository>();   

        return services;
    }
}