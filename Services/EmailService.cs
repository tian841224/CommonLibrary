using CommonLibrary.DTOs;
using CommonLibrary.Interfaces;
using System.Net;
using System.Net.Mail;

namespace CommonLibrary.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(SendEmailDto dto)
        {
            using (var client = new SmtpClient(dto.Host))
            {
                client.Port = dto.Port;
                client.Credentials = new NetworkCredential(dto.Account, dto.Password);
                client.EnableSsl = dto.EnableSsl;

                //var mailMessage = new MailMessage
                //{
                //    From = new MailAddress($"{dto.Account}@{dto.Host}", dto.DisplayName),
                //    Subject = dto.Subject,
                //    Body = dto.Body,
                //    IsBodyHtml = true,
                //};
                dto.MailMessage.To.Add(dto.Recipient);

                client.Send(dto.MailMessage);
            }
        }
    }
}
