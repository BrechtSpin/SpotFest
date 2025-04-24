using Microsoft.EntityFrameworkCore;
using ArtistService.Repositories;
using ArtistService.Messaging;
using ArtistService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<IArtistServices, ArtistServices>();

var connectionString = builder.Configuration.GetConnectionString("ArtistServiceDB");
builder.Services.AddDbContext<ArtistServiceContext>(options =>
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
    var db = scope.ServiceProvider.GetRequiredService<ArtistServiceContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
