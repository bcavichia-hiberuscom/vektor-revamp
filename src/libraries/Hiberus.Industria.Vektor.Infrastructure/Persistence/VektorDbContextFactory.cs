using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hiberus.Industria.Vektor.Infrastructure.Persistence;

public class VektorDbContextFactory : IDesignTimeDbContextFactory<VektorDbContext>
{
    public VektorDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        const string ConnectionName = "vektor";
        var connectionString =
            configuration.GetConnectionString(ConnectionName)
            ?? throw new InvalidOperationException(
                $"Connection string '{ConnectionName}' not found."
            );

        var optionsBuilder = new DbContextOptionsBuilder<VektorDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new VektorDbContext(optionsBuilder.Options);
    }
}
