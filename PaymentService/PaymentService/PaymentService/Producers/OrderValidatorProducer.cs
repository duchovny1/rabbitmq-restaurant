using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.API.Producers
{
    public static class OrderValidatorProducer
    {
        public static BlockingCollection<KeyValuePair<string, string>> respQueue
                                  = new BlockingCollection<KeyValuePair<string, string>>();

        private static string ROUTING_KEY = "validate_order";
        private static string REPLY_QUEUE_NAME = "validate_order_callback";


        private static IConnection _connection;
        private static IModel _channel;

        private static IBasicProperties props;
        private static EventingBasicConsumer _consumer;

        static OrderValidatorProducer()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(queue: ROUTING_KEY, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _consumer = new EventingBasicConsumer(_channel);

            props = _channel.CreateBasicProperties();
            props.ReplyTo = REPLY_QUEUE_NAME;
        }

        public static string ValidateOrder(string message)
        {
            var messagesBytes = Encoding.UTF8.GetBytes(message);

            var correlationId = Guid.NewGuid().ToString();
            props.CorrelationId = correlationId;

            _channel.BasicPublish("", ROUTING_KEY, props, messagesBytes);


            //var result = respQueue.FirstOrDefault(x => x.Key == correlationId).Value;

            bool isFound = false;
            KeyValuePair<string, string> result;

            while(!isFound)
            {
                respQueue.TryTake(out result);

                if (result.Key == correlationId)
                {
                    return result.Value;
                }
                else if(result.Key != null)
                {
                    respQueue.Add(result);
                }
            }

            return string.Empty;

        }
    }
}
