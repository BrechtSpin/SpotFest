using Microsoft.EntityFrameworkCore;
using HappeningService.EndPoints;
using HappeningService.Messaging;
using HappeningService.Services;
using HappeningService.Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHappeningService, HappeningServices>();

var connectionString = builder.Configuration.GetConnectionString("MusicHappeningDB");
builder.Services.AddDbContext<HappeningContext>(options =>
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
    var db = scope.ServiceProvider.GetRequiredService<HappeningContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.MapHappeningApiEndpoints();

app.Run();
