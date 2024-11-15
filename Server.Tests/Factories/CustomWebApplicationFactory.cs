using System.Data.Common;
using Bogus;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Server.Core.Data;
using Server.Core.Entities;
using Server.Core.Tests.Util;

namespace Server.Core.Tests.Factories;

public class CustomWebApplicationFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            // Create a new scope to ensure dependencies are resolved for each test run
            using var scope = services.BuildServiceProvider().CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<DataContext>();

            // Delete and recreate the database at the start of each test run
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<DataContext>));
            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbConnection));
            if (dbConnectionDescriptor != null) services.Remove(dbConnectionDescriptor);

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<DataContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });
        });
    }
}