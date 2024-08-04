namespace CommonLibrary.DTOs
{
    public class FileUploadDto
    {
        /// <summary>
        /// 檔案名稱
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// 檔案路徑
        /// </summary>
        public string FilePath { get; set; } = string.Empty;
    }
}
