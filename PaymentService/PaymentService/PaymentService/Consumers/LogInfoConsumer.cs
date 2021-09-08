using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.API.Consumers
{
    public class LogInfoConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private readonly ILogger _logger;
        private const string ROUTING_KEY = "log.info";
        private const string QUEUE_NAME = "log_info";

        public LogInfoConsumer(ILogger<LogInfoConsumer> logger)
        {
            InitializeRabbitMqListener();
            _logger = logger;
        }

        private void InitializeRabbitMqListener()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation(content);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE_NAME, false, consumer);

            return Task.CompletedTask;
        }
    }
}
