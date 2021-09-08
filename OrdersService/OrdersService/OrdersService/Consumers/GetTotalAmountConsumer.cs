using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrdersService.Business.Contracts;
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
    public class GetTotalAmountConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;
        private const string QUEUE_NAME = "order_amount";
        private const string REPLY_QUEUE_NAME = "order_amount_callback";

        private readonly IServiceProvider _sp;
        private readonly IOrdersService _orderService;

        public GetTotalAmountConsumer(IServiceProvider sp)
        {
            _sp = sp;

            using(var scope = this._sp.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IOrdersService>();
                _orderService = service;
            }

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
            _channel.QueueDeclare(queue: REPLY_QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.BasicQos(0, 1, false);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

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

                    var amount = _orderService.CalculateAmountOfOrder(long.Parse(message));

                    var responseBytes = Encoding.UTF8.GetBytes(amount.ToString());

                    _channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                         basicProperties: replyProps, body: responseBytes);

                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                else
                {
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
            };

            _channel.BasicConsume(QUEUE_NAME, false, consumer);


            return Task.CompletedTask;
        }
    }
}
