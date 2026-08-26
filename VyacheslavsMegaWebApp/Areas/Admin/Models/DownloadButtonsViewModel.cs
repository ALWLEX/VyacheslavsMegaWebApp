using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
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
