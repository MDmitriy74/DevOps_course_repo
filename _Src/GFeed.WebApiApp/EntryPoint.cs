using GFeed.WebApiApp.Implementations.HostConfigurations;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Serilog;

DotNetEnv.Env.TraversePath().Load();
var builder = WebApplication.CreateBuilder(args);

builder.AddLogConfiguration();
builder.AddServicesConfiguration();

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(
        serviceName: builder.Configuration["OTEL_SERVICE_NAME"] ?? "feedback-quiz"))
    .WithMetrics(metrics => metrics
        .AddAspNetCoreInstrumentation()
        .AddRuntimeInstrumentation()
        .AddOtlpExporter());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAppDatabase(builder.Configuration);
//builder.Services.AddSwaggerConfiguration();
//builder.Services.AddCorsConfiguration();

var app = builder.Build();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
