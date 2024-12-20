using System.Text.Json;
using Contracts;
using MassTransit;

namespace HRDirector.rabbitmq;

public class DirectorRabbitMqConsumer : IConsumer<WishlistRequest>{

    private readonly MessageAccumulator _accumulator;
    public DirectorRabbitMqConsumer(MessageAccumulator accumulator) {
        _accumulator = accumulator;
    }
    
    public Task Consume(ConsumeContext<WishlistRequest> context) {
        var wishlist = context.Message;
        Console.WriteLine($"Получено сообщение: {JsonSerializer.Serialize(wishlist)}");
        _accumulator.AddMessage(wishlist);
        return Task.CompletedTask;
    }

    public MessageAccumulator GetMessageAccumulator() {
        return _accumulator;
    }
}