using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using PaymentService.API.Producers;
using PaymentService.Domain.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.API.Consumers
{
    public class OrderValidatorConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private const string ROUTING_KEY = "validate_order";
        private const string REPLY_QUEUE_NAME = "validate_order_callback";
        private EventingBasicConsumer _consumer;

        public OrderValidatorConsumer()
        {
            InitializeRabbitMqListener();
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

            _channel.QueueDeclare(queue: REPLY_QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
        }

        public void Close()
        {
            _connection.Close();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var response = Encoding.UTF8.GetString(body);
                if (ea.BasicProperties.CorrelationId != null)
                {
                    KeyValuePair<string, string> kvp
                             = new KeyValuePair<string, string>(ea.BasicProperties.CorrelationId, response);

                    OrderValidatorProducer.respQueue.Add(kvp);
                }
            };

            _channel.BasicConsume(
            consumer: _consumer,
            queue: REPLY_QUEUE_NAME,
            autoAck: true);

            return Task.CompletedTask;
        }
    }

}
