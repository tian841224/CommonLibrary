namespace CommonLibrary.Entities
{
    /// <summary>
    /// 系統Log紀錄
    /// </summary>
    public class Log
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string Logger { get; set; } = string.Empty;
        public string Exception { get; set; } = string.Empty;
    }
}
