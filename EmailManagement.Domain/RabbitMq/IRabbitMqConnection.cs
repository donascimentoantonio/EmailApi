
using RabbitMQ.Client;

namespace EmailManagement.Domain.RabbitMq
{
    public interface IRabbitMqConnection
    {
        IConnection Connectionn { get; }
        Task<RabbitMqConnection> CreateAsync();
    }
}
