using TaskScheduler.Messaging;
using TaskScheduler.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using TaskScheduler;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<TaskSchedulerWorker>();

var connectionString = builder.Configuration.GetConnectionString("TaskSchedulerDB");
builder.Services.AddDbContext<TaskSchedulerContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddMassTransitConfiguration(builder.Configuration);
builder.Services.AddTransient<IPublisherService,PublisherService>();

var app = builder.Build();
await app.RunAsync();