using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.JSON.MCServerStat;

namespace VyacheslavsMegaWebApp.Models.ViewComponents
{
    public class FooterViewModel
    {
        public List<ServerMediaLink> ServerMediaLinks { get; set; }
        public MCServerStat MCServerStat { get; set; }
    }
}
