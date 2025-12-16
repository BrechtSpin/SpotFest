using Microsoft.EntityFrameworkCore;
using Infrastructure.OpenTelemetry;
using LoggerService.Services;
using LoggerService.Data.Repositories;
using LoggerService.Messaging;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ILoggerServices, LoggerServices>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<LoggerContext>((sp, options) =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);

builder.Services.AddOpenTelemetryService();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoggerContext>();
    db.Database.Migrate();
}

app.Run();