using Microsoft.EntityFrameworkCore;
using HappeningService.Messaging;
using HappeningService.Repositories;
using HappeningService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHappeningServices, HappeningServices>();

var connectionString = builder.Configuration.GetConnectionString("MusicHappeningDB");
builder.Services.AddDbContext<HappeningContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();

builder.Services.AddControllers();

#if DEBUG
builder.Services.AddCors(options =>
    options.AddPolicy("dev", builder =>
    {
        builder.AllowAnyOrigin();
    }));
#endif

var app = builder.Build();

#if DEBUG
app.UseCors("dev");
#endif

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<HappeningContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
