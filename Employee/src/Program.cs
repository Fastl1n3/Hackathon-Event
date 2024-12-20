using Contracts;
using Employee;
using Employee.rabbitmq;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

Console.WriteLine("Hello, World!");

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<EmployeeRabbitMqService>();
builder.Services.AddMassTransit(config => {
    config.AddConsumer<EmployeeRabbitMqConsumer>();
    config.UsingRabbitMq((ctx, cfg) => {
        cfg.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOST"), h => { });
        cfg.Message<WishlistRequest>(x => x.SetEntityName("wishlist.submissions"));
        cfg.Publish<WishlistRequest>(x => x.ExchangeType ="fanout");
        cfg.ReceiveEndpoint($"employee_notifications_queue_{Environment.GetEnvironmentVariable("APP_ID")}{Environment.GetEnvironmentVariable("APP_TYPE")}", e => {
            e.ConfigureConsumer<EmployeeRabbitMqConsumer>(ctx);
            e.Bind("event.start", x => {
                x.ExchangeType = ExchangeType.Fanout;
            });
            e.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5))); // Повторы
            e.UseInMemoryOutbox();
        });
    });
});
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Configuration.AddCommandLine(args);

var app = builder.Build();
app.Run();