var builder = DistributedApplication.CreateBuilder(args);

// API
var api = builder.AddProject<Projects.Vektor_API>("vektor-api");

// Frontend con pnpm
builder
    .AddPnpmApp("vektor-web", "../Vektor.Web", scriptName: "dev")
    .WithReference(api)
    .WaitFor(api);

builder.Build().Run();
