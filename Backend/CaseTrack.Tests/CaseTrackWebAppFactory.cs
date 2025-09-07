using System.Data.Common;
using System.Data.SqlClient;
using CaseTrack.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace CaseTrack.Tests;

public class CaseTrackWebAppFactory : WebApplicationFactory<Program>
{
    public string ConnectionString { get; set; }
    
    public CaseTrackWebAppFactory()
    {
        var databaseSetupTask = SetupDatabase();
        Task.WaitAny(databaseSetupTask);
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureServices(services =>
        {

            // Annoyingly, EF is helpful and closes connections whenever they're not needed. However the testcontainer shuts down when the last connection closes.
            // Fortunately, if we provide EF an open connection it'll reuse it - and the test container won't shut down. Win win!
            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                       typeof(DbConnection));
            services.Remove(dbConnectionDescriptor);
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new NpgsqlConnection(ConnectionString);
                connection.Open();

                return connection;
            });

            // And replace the context:
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType ==
                                                                    typeof(IDbContextOptionsConfiguration<
                                                                        CaseTrackContext>));
            services.Remove(dbContextDescriptor);
            services.AddDbContext<CaseTrackContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseNpgsql(connection);
            });

        });
    }

    private async Task SetupDatabase()
    {
        var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres")
            .Build();
        
        await postgreSqlContainer.StartAsync();

        ConnectionString = postgreSqlContainer.GetConnectionString();
    }
}