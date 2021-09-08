using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrdersService.API.Consumers
{
    public class ValidateOrderConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private const string QUEUE_NAME = "validate_order";

        public ValidateOrderConsumer()
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

            _channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);
            _channel.BasicConsume(QUEUE_NAME, false, consumer);

            consumer.Received += (ch, ea) =>
            {
                if (ea.BasicProperties.ReplyTo != null)
                {
                    var props = ea.BasicProperties;
                    var replyProps = _channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());

                    //deserealize. 
                    //then the logic here .... 

                    bool isExists = true;
                    var responseBytes = Encoding.UTF8.GetBytes(isExists.ToString());

                    _channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                         basicProperties: replyProps, body: responseBytes);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };


            return Task.CompletedTask;
        }
    }
}
