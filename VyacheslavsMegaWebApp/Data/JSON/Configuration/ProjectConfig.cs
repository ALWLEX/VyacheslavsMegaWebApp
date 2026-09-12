using VyacheslavsMegaWebApp.Data.JSON.Configuration.EmailService;
using VyacheslavsMegaWebApp.Data.JSON.Configuration.MinecraftServer;

namespace VyacheslavsMegaWebApp.Data.JSON.Configuration
{
    public class ProjectConfig
    {
        public static string ConnectionString { get; set; }
        public static MinecraftServerData MinecraftServerData { get; set; }
        public static EmailServiceConfig EmailServiceConfig { get; set; }
        public static List<string> AcceptedEmailDomains { get; set; }
    }
}
