using Microsoft.EntityFrameworkCore;
using HappeningService.EndPoints;
using HappeningService.Messaging;
using HappeningService.Services;
using HappeningService.Data.Repositories;
using HappeningService.Services.Hubs;
using HappeningService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHappeningServices, HappeningServices>();
builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
builder.Services.AddScoped<IHappeningsCurrentTimeframeService, HappeningsCurrentTimeframeService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<HappeningContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();

builder.Services.AddAuthConfig(builder.Configuration);

#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("dev", builder =>
    {    
        builder.WithOrigins("http://localhost:60000")
               .AllowAnyMethod()
               .AllowAnyHeader()
              .AllowCredentials();
    }));
#endif

var app = builder.Build();

#if DEBUG
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("dev");
#endif

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HappeningContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapHappeningApiEndpoints();

app.Run();
