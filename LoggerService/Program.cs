using LoggerService.Data.Repositories;
using LoggerService.Messaging;
using LoggerService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ILoggerServices, LoggerServices>();

builder.Services.AddMassTransitConfiguration(builder.Configuration);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<LoggerContext>((sp, options) =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LoggerContext>();
    db.Database.Migrate();
}

app.Run();