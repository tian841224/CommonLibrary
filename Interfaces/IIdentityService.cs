using CommonLibrary.DTOs;

namespace CommonLibrary.Interface
{
    public interface IIdentityService
    {
        ClaimDto GetUser();
        string GenerateToken(GenerateTokenDto dto);

    }
}
