using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.Json;

namespace EmailManagement.Application.Services
{
    public class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly EmailApiSettings _emailApiSettings;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(
            HttpClient httpClient, 
            IOptions<EmailApiSettings> emailApiSettings,
            ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _emailApiSettings = emailApiSettings.Value;
            _logger = logger;

        }
        public async Task SendEmailToApiAsync(Email email)
        {
            try
            {
                if (string.IsNullOrEmpty(_emailApiSettings.BaseUrl))
                    _logger.LogError("Url não encontrada");
                
                var url = $"{_emailApiSettings.BaseUrl}/api/EmailSender";

                var content = new StringContent(JsonSerializer.Serialize(email), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, content);

                response.EnsureSuccessStatusCode();
                _logger.LogInformation($"Email {email.Id} enviado com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
