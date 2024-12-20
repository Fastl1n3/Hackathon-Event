using HRDirector.rabbitmq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HRDirector;

public class HackathonStarter : BackgroundService {
    private const int N = 5;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public HackathonStarter(IServiceScopeFactory serviceScopeFactory) {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
        using (var scope = _serviceScopeFactory.CreateScope()) {
            var mqService = scope.ServiceProvider.GetRequiredService<DirectorRabbitMqService>();
            for (int i = 0; i < N; i++) {
                Console.WriteLine("Hackathon started!");
                await mqService.SendHackathonStart();
                Console.WriteLine("Hackathon sended!");
                 Thread.Sleep(5000);
            }   
        }
        await Task.CompletedTask;
    }
}