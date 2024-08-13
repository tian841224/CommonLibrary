using CommonLibrary.DTOs;

namespace CommonLibrary.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(SendEmailDto dto);
    }
}
