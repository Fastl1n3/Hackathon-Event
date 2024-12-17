using HRDirector;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8087");
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddDbContext<DirectorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException())
        .UseSnakeCaseNamingConvention()
        .LogTo(Console.WriteLine, LogLevel.Information));
builder.Services.AddTransient<HRDirectorService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 

var app = builder.Build();

app.UseRouting();
app.MapControllers(); 
app.Run();