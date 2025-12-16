using Microsoft.EntityFrameworkCore;
using Infrastructure.OpenTelemetry;
using JobScheduler.Messaging;
using JobScheduler.Data.Repositories;
using JobScheduler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<JobSchedulerWorker>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobSchedulerContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddScoped<IPublisherService,PublisherService>();

builder.Services.AddOpenTelemetryService();

var app = builder.Build();
await app.RunAsync();