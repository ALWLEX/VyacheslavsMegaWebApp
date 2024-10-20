using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Models.ViewComponents
{
    public class FooterViewModel
    {
        public List<ServerMediaLink> ServerMediaLinks { get; set; }
        public bool? IsServerOnline { get; set; }
        public int MaxPlayers { get; set; }
        public int OnlinePlayers { get; set; }
    }
}
