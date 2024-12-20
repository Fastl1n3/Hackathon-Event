using HRManager;
using HRManager.rabbitmq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8086");
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddSingleton<AbstractHRManager, HRManager.HRManager>();
builder.Services.AddSingleton<ITeamBuildingStrategy, DummyTeamBuildingStrategy>();
builder.Services.AddSingleton<ManagerService>();
builder.Services.AddSingleton<ManagerRabbitMqConsumer>();
builder.Services.AddMassTransit(x => {
    x.AddConsumer<ManagerRabbitMqConsumer>();
    x.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h => {
        });
        cfg.ReceiveEndpoint("wishlist_queue_manager", e => {
            e.ConfigureConsumer<ManagerRabbitMqConsumer>(ctx);
            e.Bind("wishlist.submissions", x => {
                x.ExchangeType = ExchangeType.Fanout;
            });
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5))); // Повторы
            e.UseInMemoryOutbox();
        });
        
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseRouting();
app.MapControllers(); 
app.Run();