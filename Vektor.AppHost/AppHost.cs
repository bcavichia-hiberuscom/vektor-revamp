var builder = DistributedApplication.CreateBuilder(args);

//Postgres
var postgres = builder.AddPostgres("postgres").AddDatabase("vektor");

// Migrations
var migrations = builder
    .AddProject<Projects.Vektor_MigrationService>("migrations")
    .WithReference(postgres)
    .WaitFor(postgres);

// API
var api = builder
    .AddProject<Projects.Vektor_API>("vektor-api")
    .WithReference(postgres)
    .WaitFor(migrations);

// Frontend con pnpm
builder
    .AddPnpmApp("vektor-web", "../Vektor.Web", scriptName: "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
