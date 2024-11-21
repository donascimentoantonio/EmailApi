using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;

namespace EmailManagement.Domain.Dtos.v1.Response
{
    public class EmailPostParametersResponse
    {
        public EmailId Id { get; private set; }
        public EmailStatus Status { get; private set; }
    }
}
