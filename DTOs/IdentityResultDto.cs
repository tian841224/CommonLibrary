namespace CommonLibrary.DTOs
{
    public class IdentityResultDto
    {
        public int Id { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        //public string RefreshToken { get; set; } = string.Empty;

        public long Expires { get; set; }

        public string Account { get; set; } = string.Empty;

        public int RoleId { get; set; }

        public string Photo { get; set; } = string.Empty;
    }
}
