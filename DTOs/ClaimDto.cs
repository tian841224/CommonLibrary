namespace CommonLibrary.DTOs
{
    public class ClaimDto
    {
        /// <summary>
        /// 身分ID
        /// </summary>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// 身分名稱
        /// </summary>
        public string RoleNane { get; set; } = string.Empty;

        /// <summary>
        /// 使用者ID
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string UserNane { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; } = string.Empty;

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// 更新Token ID
        /// </summary>
        public string ReferenceTokenId { get; set; } = string.Empty;
    }
}
