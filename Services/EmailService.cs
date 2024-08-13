using CommonLibrary.DTOs;
using CommonLibrary.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CommonLibrary.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmail(SendEmailDto dto)
        {
            var mailMessage = dto.MailMessage;
            mailMessage.To.Add(dto.Recipient);

            using var smtpClient = new SmtpClient(dto.Host, dto.Port)
            {
                Credentials = new NetworkCredential(dto.Account, dto.Password),
                EnableSsl = dto.EnableSsl
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
