using Microsoft.EntityFrameworkCore;
using ArtistService.Messaging;
using ArtistService.Services;
using ArtistService.EndPoints;
using ArtistService.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Services.AddScoped<IArtistServices, ArtistServices>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ArtistServiceContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddScoped<IPublisherService, PublisherService>();

#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
    options.AddPolicy("dev", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
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
    var db = scope.ServiceProvider.GetRequiredService<ArtistServiceContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.MapArtistApiEndpoints();

app.Run();
