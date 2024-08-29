using System;
using System.Text;
using RabbitMQ.Client;

public class Send
{
    private readonly string _hostname;
    private readonly string _queueName;

    public Send(string hostname, string queueName)
    {
        _hostname = hostname;
        _queueName = queueName;
    }

    public void SendMessage(string message)
    {
        var factory = new ConnectionFactory { HostName = _hostname };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName,
                             durable: false,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                             routingKey: _queueName,
                             basicProperties: null,
                             body: body);
        Console.WriteLine($" [x] Sent {message}");
    }
}

