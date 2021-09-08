using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus
{
    public class EventBus : IEventBus
    {
        private const string BROKER_NAME = "restaurant_event_bus";
        private readonly IPersistentConnection _persistentConnection;
        public EventBus(IPersistentConnection persistentConnection)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
        }

        public void Publish(string message, string topic_name)
        {
            var body = Encoding.UTF8.GetBytes(message);

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "topic");
                RegisterQueues(channel);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                channel.BasicPublish(
                        exchange: BROKER_NAME,
                        routingKey: topic_name,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
            }
        }

        public void PublishMessage(string message, string routing_key)
        {
            var body = Encoding.UTF8.GetBytes(message);

            using (var channel = _persistentConnection.CreateModel())
            {
                RegisterQueues(channel);

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2;

                channel.BasicPublish(
                        exchange: "",
                        routingKey: routing_key,
                        mandatory: true,
                        basicProperties: properties,
                        body: body);
            }
        }

        private void RegisterQueues(IModel channel)
        {
            channel.QueueDeclare(queue: "log.info", durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueDeclare(queue: "changes.history.log", durable: false, exclusive: false, autoDelete: false, arguments: null);
        }
    }
}
