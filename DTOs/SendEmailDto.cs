using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace CommonLibrary.DTOs
{
    public class SendEmailDto
    {

        [Required]
        public string Host { get; set; } = string.Empty;

        [Required]
        public int Port { get; set; }

        //[Required]
        //public string DisplayName { get; set; } = string.Empty;

        [Required]
        public string Account { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Password { get; set; } = string.Empty;

        //[Required]
        //public string Body { get; set; } = string.Empty;

        [Required]
        public string Recipient { get; set; } = string.Empty;


        [Required]
        public bool EnableSsl { get; set; }

        //[Required]
        //public string Subject { get; set; } = string.Empty;


        [Required]
        public MailMessage MailMessage { get; set; } = new();
    }
}
