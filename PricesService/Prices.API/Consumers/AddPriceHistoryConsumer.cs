using EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Prices.Business;
using Prices.Domain.ViewModels.Request;
using Prices.Domain.ViewModels.Response;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Prices.API.Consumers
{
    public class AddPriceHistoryConsumer : BackgroundService
    {
        private IModel _channel;
        private IConnection _connection;

        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private IChangesHistoryService _changesHistoryService;
        private const string ROUTING_KEY = "changes.history.log";
        private const string QUEUE_NAME = "changes.history.log";

        public AddPriceHistoryConsumer(ILogger<AddPriceHistoryConsumer> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            InitializeRabbitMqListener();
        }

        private void InitializeRabbitMqListener()
        {
            _changesHistoryService =_serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IChangesHistoryService>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QUEUE_NAME, durable: false, exclusive: false, autoDelete: false, arguments: null);

            _channel.QueueBind(queue: QUEUE_NAME,
                                 exchange: "restaurant_event_bus",
                                 routingKey: ROUTING_KEY);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());

                if (string.IsNullOrEmpty(content))
                {
                    throw new ArgumentNullException();
                }

                var changesHistory = JsonConvert.DeserializeObject<ProductPriceResponse>(content);

                var priceModel = new SetProductPriceRequest
                { ProductName = changesHistory.ProductName, Price = changesHistory.PriceAmount };


                _changesHistoryService.AddToHistory(changesHistory.ProductId, priceModel);

                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(QUEUE_NAME, false, consumer);

            return Task.CompletedTask;
        }
    }
}
