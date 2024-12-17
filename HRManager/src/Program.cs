using HRManager;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8086");
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<AbstractHRManager, HRManager.HRManager>();
builder.Services.AddSingleton<ITeamBuildingStrategy, DummyTeamBuildingStrategy>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseRouting();
app.MapControllers(); 
app.Run();