using CommonLibrary.DTOs;

namespace CommonLibrary.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(SendEmailDto dto);
    }
}
