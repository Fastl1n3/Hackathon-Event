using System.Text.Json;
using Contracts;
using MassTransit;

namespace Employee.rabbitmq;

public class EmployeeRabbitMqConsumer : IConsumer<HackathonStarted> {
    private readonly EmployeeService _employeeService;

    public EmployeeRabbitMqConsumer(EmployeeService employeeService) {
        _employeeService = employeeService;
    }

    public Task Consume(ConsumeContext<HackathonStarted> context) {
        var message = context.Message;
        Console.WriteLine($"Получено сообщение: {JsonSerializer.Serialize(message)}");
        _employeeService.GoToHackathon();

        return Task.CompletedTask;
    }
}