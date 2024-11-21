using EmailManagement.Domain.Models.Email;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace EmailManagement.Domain.Dtos.v1.Request
{
    [ExcludeFromCodeCoverage]
    public class EmailPostParametersRequest
    {
        [Required]
        public string Sender { get; set; }

        [Required]
        public List<string> Recipients { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public IReadOnlyList<Atachment>? Atachments { get; private set; }

        public bool Send { get; set; } = false;

        public static EmailPostParametersRequest Create(
            string sender,
            List<string> recipients,
            string subject, string body,
            List<Atachment>? atachments = null)
        {
            return new EmailPostParametersRequest
            {
                Sender = sender,
                Recipients = recipients,
                Subject = subject,
                Body = body,
                Atachments = atachments ?? new List<Atachment>(),
            };
        }
    }
}
