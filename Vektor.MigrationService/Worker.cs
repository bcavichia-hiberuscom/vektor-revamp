using Microsoft.EntityFrameworkCore;
using Vektor.API.Infrastructure.Persistence;

namespace Vektor.MigrationService;

public class Worker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly ILogger<Worker> _logger;

    public Worker(
        IServiceProvider serviceProvider,
        IHostApplicationLifetime lifetime,
        ILogger<Worker> logger
    )
    {
        _serviceProvider = serviceProvider;
        _lifetime = lifetime;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Applying migrations...");

        using var scope = _serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<VektorDbContext>();

        await db.Database.MigrateAsync(stoppingToken);

        _logger.LogInformation("Migrations applied successfully.");

        _lifetime.StopApplication();
    }
}
