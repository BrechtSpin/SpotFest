using Microsoft.EntityFrameworkCore;
using Infrastructure.OpenTelemetry;
using UserAuthService;
using UserAuthService.Data.Repositories;
using UserAuthService.EndPoints;
using UserAuthService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IAuthService, AuthService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextFactory<UserAuthContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddAuthIdentityConfig(builder.Configuration);
builder.Services.AddAuthorization();

builder.Services.AddOpenTelemetryService();

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
    var db = scope.ServiceProvider.GetRequiredService<UserAuthContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapAuthApiEndpoints();

app.Run();
