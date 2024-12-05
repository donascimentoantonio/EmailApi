using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Enum;
using EmailManagement.Domain.Models.Email;
using EmailManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace EmailManagement.Api.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailsController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailsController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        /// <summary>
        /// Faz a persistência de múltiplos e-mails e os envia caso necessário.
        /// </summary>
        /// <param name="requests">Lista de requisições de e-mail</param>
        /// <returns>Lista de e-mails com status</returns>
        [HttpPost("salvar")]
        public async Task<ActionResult<IEnumerable<EmailPostParametersResponse>>> SendEmail([FromBody] List<EmailPostParametersRequest> requests)
        {
            // Chama o serviço para enviar os e-mails em lote
            var result = await _emailService.SendEmailsAsync(requests);

            return Ok(result);
        }

        /// <summary>
        /// Recupera e-mails enviados entre duas datas.
        /// </summary>
        /// <param name="startDate">Data de início</param>
        /// <param name="endDate">Data de fim</param>
        /// <returns>Lista de e-mails encontrados</returns>
        [HttpGet("emails-por-data")]
        public async Task<ActionResult<IEnumerable<EmailGetParametersResponse>>> GetEmailsByDate(DateTime startDate, DateTime endDate)
        {
            var result = await _emailService.GetEmailsByDateAsync(startDate, endDate);
            return Ok(result);
        }

        /// <summary>
        /// Realiza a busca de e-mails com base em um filtro dinâmico.
        /// </summary>
        /// <param name="filter">Objeto com os filtros opcionais</param>
        /// <returns>Lista de e-mails encontrados</returns>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmailGetParametersResponse>>> SearchEmails([FromQuery] SearchEmailFilterRequest filter)
        {
            // Chama o serviço para realizar a busca com os filtros
            var result = await _emailService.SearchEmailsAsync(filter);

            return Ok(result);
        }
    }
}
