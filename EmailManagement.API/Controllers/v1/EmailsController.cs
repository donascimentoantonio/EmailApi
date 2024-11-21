using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using Microsoft.AspNetCore.Mvc;

namespace EmailManagement.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        /// <summary>
        /// Faz a persistencia do email e envia caso necessário
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("salvar")]
        public async Task<ActionResult<EmailPostParametersResponse>> SendEmail(EmailPostParametersRequest request)
        {
                //var input = new SendEmailInput(
                //    request.Sender,
                //    request.Recipients,
                //    request.Subject,
                //    request.Body,
                //    request.Atachments);

                //await _sendEmailUseCase.Execute(input);

                return Ok();
        }
    }
}
