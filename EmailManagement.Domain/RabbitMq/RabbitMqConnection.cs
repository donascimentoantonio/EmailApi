using RabbitMQ.Client;

namespace EmailManagement.Domain.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private readonly IConnection? _connection;
        public IConnection Connectionn => _connection!;

        private RabbitMqConnection(IConnection connection)
        {
            _connection = connection;
        }

        public async Task<RabbitMqConnection> CreateAsync()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            var connection = await factory.CreateConnectionAsync();
            return new RabbitMqConnection(connection);
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }

}
