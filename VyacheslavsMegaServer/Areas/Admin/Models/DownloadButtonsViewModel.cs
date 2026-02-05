using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Areas.Admin.Models
{
    public class DownloadButtonsViewModel
    {
        public DownloadButtonsViewModel(List<DownloadButton> downloadButtons)
        {
            DownloadButtons = downloadButtons;
        }

        public List<DownloadButton> DownloadButtons { get; set; }
    }
}
