using CommonLibrary.DTOs;

namespace CommonLibrary.Interface
{
    public interface IEmailService
    {
        void SendEmail(SendEmailDto dto);
    }
}
