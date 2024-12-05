using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;
using System.Text.Json.Serialization;

namespace EmailManagement.Domain.Dtos.v1.Response
{
    public class EmailPostParametersResponse
    {
        public string Id { get;  set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmailStatus Status { get;  set; }
    }
    
    public class EmailGetParametersResponse
    {
        public string Id { get;  set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public EmailStatus Status { get;  set; }

        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IReadOnlyList<Atachment>? Atachments { get; private set; }
        public DateTime? SentAt { get; private set; }
        public int Attempts { get; private set; }
        public DateTime? LastAttemptAt { get; private set; }
    }
}
