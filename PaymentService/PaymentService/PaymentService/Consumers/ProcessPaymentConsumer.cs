using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PaymentService.API.Producers;
using PaymentService.Domain;
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
    public class ProcessPaymentConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private readonly ILogger _logger;
        private const string QUEUE_NAME = "process_payment";

        public ProcessPaymentConsumer(ILogger<ProcessPaymentConsumer> logger)
        {
            _logger = logger;

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
                var orderNumber = Encoding.UTF8.GetString(ea.Body.ToArray());

                var response = OrderValidatorProducer.ValidateOrder(orderNumber);

                var isOrderValidate = bool.Parse(response);

                var pricesValidator = GetTotalAmountProducer.ValidateOrder(orderNumber);

                decimal amount = 0.0m;

                if (isOrderValidate && pricesValidator != null)
                {
                    decimal.TryParse(pricesValidator, out amount);
                };

                _logger.LogInformation($"Order - {orderNumber} was paid total amount of {amount}");
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE_NAME, false, consumer);

            return Task.CompletedTask;
        }
    }
}
