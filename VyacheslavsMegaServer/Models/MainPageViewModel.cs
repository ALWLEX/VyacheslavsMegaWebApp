using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Data.Entities;
using VyacheslavsMegaServer.Data.JSON.MCServerStat;
using VyacheslavsMegaServer.Data.Repositories;
using VyacheslavsMegaServer.Service;

namespace VyacheslavsMegaServer.Models
{
    public class MainPageViewModel
    {
        public MainPageData MainPageData { get; set; }

        public MCServerStat MCServerStat { get; set; }

        public List<ServerMediaLink> MediaLinks { get; set; }
        
        public List<Partner> PartnersData { get; set; }
        
        public Contact Creator { get; set; }
    }
}
