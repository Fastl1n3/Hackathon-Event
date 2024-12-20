using Contracts;
using MassTransit;

namespace Employee.rabbitmq;

public class EmployeeRabbitMqService {
    private readonly IBusControl _busControl;
    private readonly IPublishEndpoint _publishEndpoint;

    public EmployeeRabbitMqService(IBusControl busControl, IPublishEndpoint publishEndpoint) {
        _busControl = busControl;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task SendWishlistAsync(WishlistRequest wishlist) {
        if (_busControl is null) {
            throw new InvalidOperationException("The bus control is not initialized.");
        }
        await _publishEndpoint.Publish(wishlist);
    }
}