using CommonLibrary.DTOs;

namespace CommonLibrary.Interfaces
{
    public interface IIdentityService
    {
        ClaimDto GetUser();
        string GenerateToken(GenerateTokenDto dto);

    }
}
