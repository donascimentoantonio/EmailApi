using System.Diagnostics.CodeAnalysis;

namespace EmailManagement.Api.Components
{
    public class ErrorResponse
    {
        public ErrorResponse()
        {
        }

        public ErrorResponse(int statusCode, string message)
        {
            StatusCode = statusCode;

            Error = new ObjectError(message, new ObjectDetailsError(0, string.Empty));
        }

        public ErrorResponse(int statusCode, string message, int code)
        {
            StatusCode = statusCode;

            Error = new ObjectError(message, new ObjectDetailsError(code, string.Empty));
        }

        public ErrorResponse(int statusCode, string message, int code, string? reason)
        {
            StatusCode = statusCode;

            Error = new ObjectError(message, new ObjectDetailsError(code, reason ?? string.Empty));
        }

        public int StatusCode { get; set; }

        public ObjectError Error { get; set; }
    }



    [ExcludeFromCodeCoverage]

    public class ObjectError
    {
        public ObjectError(string? message, ObjectDetailsError details)
        {
            Message = message;

            Details = details;
        }

        public string? Message { get; set; }

        public ObjectDetailsError Details { get; set; }
    }

    [ExcludeFromCodeCoverage]

    public class ObjectDetailsError
    {
        public ObjectDetailsError(int? code, string? reason)
        {
            Code = code;

            Reason = reason;
        }

        public int? Code { get; private set; }

        public string? Reason { get; private set; }

    }
}
