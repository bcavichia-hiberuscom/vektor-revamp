var builder = DistributedApplication.CreateBuilder(args);

//Postgres
var postgres = builder.AddPostgres("postgres").AddDatabase("vektor");

// API
var api = builder
    .AddProject<Projects.Vektor_API>("vektor-api")
    .WithReference(postgres)
    .WaitFor(postgres);

// Frontend con pnpm
builder
    .AddPnpmApp("vektor-web", "../Vektor.Web", scriptName: "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
