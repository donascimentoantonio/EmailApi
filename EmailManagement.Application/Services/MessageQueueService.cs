using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.RabbitMq;
using EmailManagement.Domain.Services;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EmailManagement.Application.Services
{
    public class RabbitMqMessageQueueService : IMessageQueueService
    {
        private readonly string _queueName = "email_queue";
        private readonly string _hostName = "localhost"; // Configure conforme necessário
        private readonly ILogger<RabbitMqMessageQueueService> _logger;

        private readonly IRabbitMqConnection _connection;

        public RabbitMqMessageQueueService(IRabbitMqConnection connection,
            ILogger<RabbitMqMessageQueueService> logger)
        {
            _connection = connection;
            _logger = logger;

        }

        public async Task EnqueueEmail(Email email)
        {
            try
            {
                // Garantir que a conexão esteja estabelecida antes de usar
                if (_connection.Connectionn == null || !_connection.Connectionn.IsOpen)
                {
                    await _connection!.CreateAsync();
                }
                using var channel = await _connection.Connectionn!.CreateChannelAsync();

                await channel.QueueDeclareAsync(queue: _queueName,
                    durable: true,   // Persistência
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var json = JsonSerializer.Serialize(new
                {
                    Id = email.Id.Value,
                    Sender = email.Sender,
                    Recipients = email.Recipients,
                    Subject = email.Subject,
                    Body = email.Body,
                    Atachments = email.Atachments,
                });
                var body = Encoding.UTF8.GetBytes(json);

                await channel.BasicPublishAsync(exchange: _queueName, routingKey: _hostName, body: body);
                _logger.LogInformation($"Email {email.Id} enfileirado com sucesso.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao enfileirar o e-mail {email.Id}: {ex.Message}");
                throw;
            }
        }
    }

}
