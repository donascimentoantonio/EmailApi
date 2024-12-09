using EmailManagement.Domain.Dtos.v1.Request;
using EmailManagement.Domain.Dtos.v1.Response;
using EmailManagement.Domain.Services;
using Microsoft.AspNetCore.Mvc;

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

        /// <summary>
        /// Atualizar o email
        /// </summary>
        /// <param name="request">Lista de requisições de e-mail</param>
        /// <param name="id">Email id para atualização</param>
        /// <returns>Atualizar alguma informação do email</returns>
        [HttpPut("update")]
        public async Task<ActionResult<EmailPostParametersResponse>> UpdateEmailAsync(Guid emailId,
            [FromBody] EmailPostParametersRequest request)
        {
            var updatedEmailResponse = await _emailService.UpdateEmailAsync(emailId, request);
            if (updatedEmailResponse == null)
            {
                return NotFound(new { Message = $"Email com ID '{emailId}' não foi encontrado." });
            }

            return Ok();
        }

        /// <summary>
        /// Recuperar um e-mail por ID
        /// </summary>
        /// <param name="id">ID do e-mail para recuperação</param>
        /// <returns>Informações detalhadas do e-mail</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EmailGetParametersResponse>> GetEmailByIdAsync(Guid emailId)
        {
            var emailResponse = await _emailService.GetEmailByIdAsync(emailId);

            if (emailResponse == null)
            {
                return NotFound(new { Message = $"Email com ID '{emailId}' não foi encontrado." });
            }

            return Ok(emailResponse);
        }

        /// <summary>
        /// Envia e-mails pendentes
        /// </summary>
        /// <returns>Quantidade de e-mails enviados</returns>
        [HttpPost("send-pending")]
        public async Task<ActionResult<int>> SendPendingEmailsAsync()
        {
            try
            {
                //var emailsSentCount = await _emailService.SendPendingEmailsAsync();
                //return Ok(new { EmailsSent = emailsSentCount });
                return Ok();
            }
            catch (Exception ex)
            {
                // Log de erro pode ser incluído aqui
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Erro ao enviar e-mails pendentes.", Details = ex.Message });
            }
        }

    }
}
