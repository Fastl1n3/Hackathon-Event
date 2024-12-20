using System.Text.Json;
using Contracts;
using MassTransit;

namespace HRManager.rabbitmq;

public class ManagerRabbitMqConsumer : IConsumer<WishlistRequest> {
    private readonly ManagerService _managerService;

    public ManagerRabbitMqConsumer(ManagerService managerService) {
        _managerService = managerService;
    }

    public Task Consume(ConsumeContext<WishlistRequest> context) {
        var wishlist = context.Message;
        Console.WriteLine($"Получено сообщение: {JsonSerializer.Serialize(wishlist)}");
        _managerService.HandleWishlistRequest(wishlist);
        
        return Task.CompletedTask;
    }
}