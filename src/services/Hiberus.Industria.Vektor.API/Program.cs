using System.Reflection;
using System.Text.Json.Serialization;
using Hiberus.Industria.Vektor.Infrastructure;
using Hiberus.Industria.Vektor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    options.IncludeXmlComments(xmlPath);
});

builder.AddNpgsqlDbContext<VektorDbContext>(Constants.DatabaseConnectionName);

builder.Services.AddInfrastructureServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vektor API v1");
    });
}

app.UseAuthorization();

app.MapControllers();

app.Run();
