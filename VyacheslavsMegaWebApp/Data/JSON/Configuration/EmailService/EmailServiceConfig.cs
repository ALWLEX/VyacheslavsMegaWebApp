namespace VyacheslavsMegaWebApp.Data.JSON.Configuration.EmailService
{
    public class EmailServiceConfig
    {
        public static string SenderName { get; set; }
        public static string Email { get; set; }
        public static string Password { get; set; }
        public static SmtpServerConfig SmtpServerConfig { get; set; }
    }
}