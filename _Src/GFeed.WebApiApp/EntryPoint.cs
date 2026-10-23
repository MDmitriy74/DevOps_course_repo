using GFeed.WebApiApp.Implementations.HostConfigurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogConfiguration();
builder.AddServicesConfiguration();
DotNetEnv.Env.TraversePath().Load();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddAppDatabase(builder.Configuration);
//builder.Services.AddSwaggerConfiguration();
//builder.Services.AddCorsConfiguration();

var app = builder.Build();

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