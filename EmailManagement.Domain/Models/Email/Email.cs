using EmailManagement.Domain.Enum;

namespace EmailManagement.Domain.Models.Email
{
    public class Email
    {
        public EmailId Id { get; set; }
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public IReadOnlyList<Atachment>? Atachments { get; private set; }
        public DateTime? SentAt { get; private set; }
        public EmailStatus Status { get; private set; }
        public int Attempts { get; private set; }
        public DateTime? LastAttemptAt { get; private set; }

        public static Email Create(
            string sender,
            List<string> recipients,
            string subject, string body,
            List<Atachment>? atachments = null)
        {
            return new Email
            {
                Id = new EmailId(Guid.NewGuid()),
                Sender = sender,
                Recipients = recipients,
                Subject = subject,
                Body = body,
                Atachments = atachments ?? new List<Atachment>(),
                Status = EmailStatus.Pending,
                Attempts = 0
            };
        }

        public void MarkAsSent()
        {
            Status = EmailStatus.Sent;
            SentAt = DateTime.UtcNow;
        }

        public void MarkAsPending()
        {
            Status = EmailStatus.Pending;
            Attempts++;
            LastAttemptAt = DateTime.UtcNow;
        }

        public void MarkAsError()
        {
            Status = EmailStatus.Error;
            LastAttemptAt = DateTime.UtcNow;
        }
    }
}
