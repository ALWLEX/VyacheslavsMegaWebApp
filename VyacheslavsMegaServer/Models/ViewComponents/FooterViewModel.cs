using VyacheslavsMegaServer.Data.Entities;
using VyacheslavsMegaServer.Data.JSON.MCServerStat;

namespace VyacheslavsMegaServer.Models.ViewComponents
{
    public class FooterViewModel
    {
        public List<ServerMediaLink> ServerMediaLinks { get; set; }
        public MCServerStat MCServerStat { get; set; }
    }
}
