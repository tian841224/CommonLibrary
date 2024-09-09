namespace admin_backend.DTOs.MailConfig
{
    public class MailSettings
    {
        public string Account { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string SenderMail { get; set; }
        public string MailSubject { get; set; }
        public string MailSubjectPrefix { get; set; }
        public string OnlyMailTo { get; set; }
    }
}
