using System.Text;
using Contracts;
using MassTransit;

namespace HRDirector.rabbitmq;

public class DirectorRabbitMqService {
    private readonly IBusControl _busControl;
    private readonly IPublishEndpoint _publishEndpoint;

    public DirectorRabbitMqService(IBusControl busControl, IPublishEndpoint publishEndpoint) {
        _busControl = busControl;
        _publishEndpoint = publishEndpoint;
    }

    public async Task SendHackathonStart() {
        if (_busControl is null) {
            throw new InvalidOperationException("The bus control is not initialized.");
        }
        await _publishEndpoint.Publish(new HackathonStarted("Go to hackathon!"));
    }
}