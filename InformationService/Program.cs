using InformationService.Data.Repositories;
using InformationService.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateSlimBuilder(args);
//var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContactContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

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
app.MapArtistApiEndpoints();

app.Run();

