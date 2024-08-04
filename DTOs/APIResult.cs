namespace CommonLibrary.DTOs
{
    public class APIResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 資料
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        public string? ErrorMsg { get; set; } = string.Empty;
    }
}
