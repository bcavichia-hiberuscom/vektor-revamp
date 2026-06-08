var builder = DistributedApplication.CreateBuilder(args);

// PostgreSQL
var postgres = builder
    .AddPostgres("postgres", port: 5434)
    .WithDataVolume("vektor-postgres-data")
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("vektor");

// Migrations
var migrations = builder
    .AddProject<Projects.Hiberus_Industria_Vektor_MigrationService>("migrations")
    .WithReference(db)
    .WaitFor(db);

// API
var api = builder
    .AddProject<Projects.Hiberus_Industria_Vektor_API>("vektor-api")
    .WithReference(db)
    .WaitFor(migrations);

// Frontend
builder
    .AddPnpmApp("vektor-client", "../Services/Hiberus.Industria.Vektor.Client", scriptName: "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
