using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CommonLibrary.DTOs
{
    public class JwtConfig
    {
        public string SecretKey { get; set; } = string.Empty;

        public string Issuer { get; set; } = string.Empty;

        public string Audience { get; set; } = string.Empty;

        public int Expired { get; set; }

        public int RememberLoginExpired { get; set; }

        public DateTime NotBefore => DateTime.Now;

        public DateTime IssuedAt => DateTime.Now;

        public DateTime Expiration => IssuedAt.AddMinutes(Expired);

        public DateTime RememberLoginExpiration => IssuedAt.AddDays(RememberLoginExpired);

        public SecurityKey SigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        public SigningCredentials SigningCredentials => new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);

    }
}
