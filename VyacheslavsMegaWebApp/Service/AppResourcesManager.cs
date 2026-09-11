namespace VyacheslavsMegaWebApp.Service
{
    public static class AppResourcesManager
    {
        #region EmailTemplates

        public enum EmailMessageFormat { Html, Text }

        public const string EmailConfirmation = "EmailConfirmation";
        public const string PasswordReset = "PasswordReset";

        #endregion

        #region Images

        public const string EmailLogo = "logoEmail.png";

        #endregion

        public static string AppResourcesFolder => Path.Combine(AppContext.BaseDirectory, "AppResources");

        public static string GetImage(string imageName)
        {
            return Path.Combine(AppResourcesFolder, "Images", imageName);
        }

        public static async Task<string> GetEmailTemplate(string templateName, EmailMessageFormat format)
        {
            return await File.ReadAllTextAsync(Path.Combine(
                AppResourcesFolder,
                "EmailTemplates",
                templateName,
                string.Concat("Template", format == EmailMessageFormat.Html ? ".html" : ".txt")));
        }
    }
}
