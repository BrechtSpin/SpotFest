using JobScheduler.Messaging;
using JobScheduler.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using JobScheduler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<JobSchedulerWorker>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobSchedulerContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddTransient<IPublisherService,PublisherService>();

var app = builder.Build();
await app.RunAsync();