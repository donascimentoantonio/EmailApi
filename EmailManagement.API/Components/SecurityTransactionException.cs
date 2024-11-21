
namespace EmailManagement.Api.Components
{
    public class SecurityTransactionException : Exception
    {
        public object Response{ get; set; }
        public int? StatusCode { get; set; }

        public SecurityTransactionException(object response, int? statusCode) 
        {
            Response = response;
            StatusCode = statusCode;
        }
    }
}
