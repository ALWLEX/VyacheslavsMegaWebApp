using System.Reflection;
using System.Resources;

namespace VyacheslavsMegaWebApp.Service
{
    public class AppResourcesManager
    {
        public AppResourcesManager()
        {
            IdentityErrors = new ResourceManager("AppResources.Resx.IdentityErrors", Assembly.GetExecutingAssembly());
            SharedResources = new ResourceManager("AppResources.Resx.IdentityErrors", Assembly.GetExecutingAssembly());
        }

        #region Resx resousces

        public ResourceManager IdentityErrors { get; private set; }

        public ResourceManager SharedResources { get; private set; }

        #endregion

        #region EmailTemplates

        public enum EmailMessageFormat { Html, Text }

        public const string EmailConfirmation = "EmailConfirmation";
        public const string PasswordReset = "PasswordReset";

        #endregion

        #region Images

        public const string EmailLogo = "logoEmail.png";

        #endregion

        public string AppResourcesFolder => Path.Combine(AppContext.BaseDirectory, "AppResources");

        public string GetImage(string imageName)
        {
            return Path.Combine(AppResourcesFolder, "Images", imageName);
        }

        public async Task<string> GetEmailTemplate(string templateName, EmailMessageFormat format)
        {
            return await File.ReadAllTextAsync(Path.Combine(
                AppResourcesFolder,
                "EmailTemplates",
                templateName,
                string.Concat("Template", format == EmailMessageFormat.Html ? ".html" : ".txt")));
        }
    }
}
