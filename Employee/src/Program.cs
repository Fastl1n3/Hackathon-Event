using Employee;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);
//builder.WebHost.UseUrls("http://localhost:8087");
builder.Services.AddHttpClient();
//builder.Services.AddControllers();
builder.Services.AddHostedService<EmployeeService>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration.AddCommandLine(args);

var app = builder.Build();
Console.WriteLine("ARGS " + args);
//app.UseRouting();
//app.MapControllers(); 
app.Run();