var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL
var postgres = builder.AddPostgres("postgres").AddDatabase("vektor");

// Migrations
var migrations = builder
    .AddProject<Projects.Hiberus_Industria_Vektor_MigrationService>("migrations")
    .WithReference(postgres)
    .WaitFor(postgres);

// API
var api = builder
    .AddProject<Projects.Hiberus_Industria_Vektor_API>("vektor-api")
    .WithReference(postgres)
    .WaitFor(migrations);

// Frontend
builder
    .AddPnpmApp("vektor-client", "../Services/Hiberus.Industria.Vektor.Client", scriptName: "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
