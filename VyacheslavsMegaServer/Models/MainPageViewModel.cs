using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Data.Entities;
using VyacheslavsMegaServer.Data.Repositories;

namespace VyacheslavsMegaServer.Models
{
    public class MainPageViewModel
    {
        public MainPageData MainPageData { get; set; }

        public List<ServerMediaLink> MediaLinks { get; set; }
        
        public List<Partner> PartnersData { get; set; }
        
        public Contact Creator { get; set; }
    }
}
