using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.JSON.MCServerStat;
using VyacheslavsMegaWebApp.Data.Repositories;
using VyacheslavsMegaWebApp.Service;

namespace VyacheslavsMegaWebApp.Models
{
    public class MainPageViewModel
    {
        public MainPageData MainPageData { get; set; }

        public MCServerStat MCServerStat { get; set; }

        public List<DownloadButton> DownloadButtons { get; set; }

        public List<ServerMediaLink> MediaLinks { get; set; }
        
        public List<Partner> PartnersData { get; set; }
        
        public Contact Creator { get; set; }
    }
}
