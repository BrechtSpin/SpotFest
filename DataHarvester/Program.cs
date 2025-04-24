using DataHarvester.SpotifyWeb;
using DataHarvester.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHttpClient<SpotifyWebApiClientTokenClient>();
builder.Services.AddHttpClient<ISpotifyWebApiClient, SpotifyWebApiClient>();
builder.Services.AddSingleton<SpotifyWebApiClientTokenClient>();
builder.Services.AddScoped<ISpotifyWebApiClient, SpotifyWebApiClient>();
builder.Services.AddScoped<ISpotifyWebScraper, SpotifyWebScraperAngleSharp>();
builder.Services.AddScoped<SpotifyHarvester>();


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
// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
