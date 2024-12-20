using Contracts;
using HRDirector;
using HRDirector.rabbitmq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;


var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://0.0.0.0:8087");
builder.Services.AddHttpClient();
builder.Services.AddControllers();
builder.Services.AddDbContext<DirectorDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException())
        .UseSnakeCaseNamingConvention()
        .LogTo(Console.WriteLine, LogLevel.Information));
builder.Services.AddScoped<HRDirectorService>();
builder.Services.AddScoped<DirectorRabbitMqService>();
builder.Services.AddSingleton<MessageAccumulator>();
builder.Services.AddHostedService<HackathonStarter>();
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 

builder.Services.AddMassTransit(x => {
    x.AddConsumer<DirectorRabbitMqConsumer>();
    x.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h => {
        });
        cfg.Message<HackathonStarted>(x => x.SetEntityName("event.start"));
        cfg.Publish<HackathonStarted>(x => x.ExchangeType ="fanout");
        cfg.ReceiveEndpoint("wishlist_queue_director", e => {
            e.ConfigureConsumer<DirectorRabbitMqConsumer>(ctx);
            e.Bind("wishlist.submissions", x => {
                x.ExchangeType = ExchangeType.Fanout;
            });
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5))); // Повторы
            e.UseInMemoryOutbox();
        });
    });
});

var app = builder.Build();

app.UseRouting();
app.MapControllers(); 
app.Run();