namespace CommonLibrary.DTOs
{
    public class CaptchaResult
    {
        public string CaptchaCode { get; set; } = string.Empty;

        public byte[] CaptchaByteData { get; set; } = Array.Empty<byte>();

        public string CaptchaBase64Data => Convert.ToBase64String(CaptchaByteData);

        public DateTime Timestamp { get; set; }
    }
}
