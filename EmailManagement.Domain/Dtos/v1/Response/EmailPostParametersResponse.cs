using EmailManagement.Domain.Enum;

namespace EmailManagement.Domain.Dtos.v1.Response
{
    public class EmailPostParametersResponse
    {
        public string Id { get; private set; }
        public EmailStatus Status { get; private set; }
    }
}
