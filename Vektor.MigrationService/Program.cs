using Vektor.API.Infrastructure.Persistence;
using Vektor.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddNpgsqlDbContext<VektorDbContext>("vektor");
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
