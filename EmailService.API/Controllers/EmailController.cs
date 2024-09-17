using EmailService.API.Contracts;
using EmailService.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.API.Controllers
{
    [ApiController]
    [Route("[action]")]
    public class EmailController(
        IOptions<EmailOptions> options,
        ILogger<EmailController> logger) : ControllerBase
    {
        private readonly EmailOptions _options = options.Value;
        private readonly ILogger<EmailController> _logger = logger;

        [HttpPost]
        public async Task<ActionResult> SendEmail(Email email)
        {
            _logger.LogInformation($"Email: {email.EmailAddress}; Subject: {email.Subject}; Sender: {email.Sender}",
                DateTime.Now.ToLongTimeString());
            try
            {
                using var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(email.Sender, _options.Login));
                emailMessage.To.Add(new MailboxAddress("", email.EmailAddress));
                emailMessage.Subject = email.Subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = email.Message
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    await client.ConnectAsync(_options.Service, _options.Port, _options.UseSsl);
                    await client.AuthenticateAsync(_options.Login, _options.Password);
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString(),
                    DateTime.Now.ToLongTimeString());
            }

            return Ok();
        }
    }
}
