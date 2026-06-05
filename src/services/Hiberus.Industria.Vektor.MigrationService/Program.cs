using Hiberus.Industria.Vektor.Infrastructure.Persistence;
using Hiberus.Industria.Vektor.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<VektorDbContext>("vektor");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
