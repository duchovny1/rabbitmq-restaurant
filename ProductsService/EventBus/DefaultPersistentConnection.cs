using RabbitMQ.Client;
using System;

namespace EventBus
{
    public class DefaultPersistentConnection : IPersistentConnection
    {
        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _isDisposed;
        public bool IsConnected => _connection != null && _connection.IsOpen && !_isDisposed;

        public DefaultPersistentConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }
        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;

            // should add it in a try catch block
            _connection.Close();
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (Exception)
            {
                throw new Exception("FATAL ERROR: RabbitMQ connections could not be created and opened");
            }

            if (!IsConnected)
            {
                return false;
            }

            return true;
        }
    }
}
