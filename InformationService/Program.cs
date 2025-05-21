using InformationService.Services;
using InformationService.Data.Repositories;
using InformationService.Endpoints;
using Microsoft.EntityFrameworkCore;
using InformationService.Extension;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContactContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddEnvironmentSettings();
builder.Services.AddScoped<IContactServices,ContactServices>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddEmailRateLimiter();

#if DEBUG

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

app.UseCors("dev");
#endif

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ContactContext>();
    db.Database.Migrate();
}

app.UseRateLimiter();
app.MapContactApiEndpoints();

app.Run();

