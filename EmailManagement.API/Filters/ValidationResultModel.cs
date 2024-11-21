using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using EmailManagement.Api.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace EmailManagement.Api.Filters
{
    public class ValidationFailedResult : ObjectResult
    {
        public ValidationFailedResult(ModelStateDictionary modelState) : base(new ValidationResultModel(modelState))
        {
            StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public class ValidationResultModel
    {
        public int StatusCode { get; set; }

        public ObjectError Error { get; }


        public ValidationResultModel(ModelStateDictionary modelState)
        {
            StatusCode = StatusCodes.Status400BadRequest;

            var key = modelState.FirstOrDefault(p => !p.Key.Equals(string.Empty!) && p.Value.ValidationState.Equals(ModelValidationState.Invalid)).Key;

            Error = new ObjectError(ReasonPhrases.GetReasonPhrase(StatusCode), new ObjectDetailsError(0, $"{key} invalido"));
        }
    }
}
