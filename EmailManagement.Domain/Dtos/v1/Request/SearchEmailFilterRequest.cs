using EmailManagement.Domain.Enum;

namespace EmailManagement.Domain.Dtos.v1.Request
{
    public class SearchEmailFilterRequest
    {
        public string? Sender { get; set; }
        public EmailStatus? Status { get; set; }
        public List<string>? Recipients { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public DateTime? SentAfter { get; set; }
        public DateTime? SentBefore { get; set; }
        public int? MinAttempts { get; set; }
        public int? MaxAttempts { get; set; }
    }
}
