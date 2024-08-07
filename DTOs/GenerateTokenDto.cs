namespace CommonLibrary.DTOs
{
    /// <summary>
    /// Jwt Token 資訊
    /// </summary>
    public class GenerateTokenDto
    {
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// 帳號
        /// </summary>
        //public string Account { get; set; } = string.Empty;

        ///// <summary>
        ///// 姓名
        ///// </summary>
        //public string Name { get; set; } = string.Empty;

        ///// <summary>
        ///// 信箱
        ///// </summary>
        //public string Email { get; set; } = string.Empty;

        ///// <summary>
        ///// 身分
        ///// </summary>
        //public string Role { get; set; } = string.Empty;

        ///// <summary>
        ///// 更新Tokem
        ///// </summary>
        //public string RefreshToken { get; set; } = string.Empty;

        /// <summary>
        /// Token參數
        /// </summary>
        public ClaimDto Claims { get; set; } = new();
    }
}
