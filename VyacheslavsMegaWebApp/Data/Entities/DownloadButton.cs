using System.ComponentModel.DataAnnotations;

namespace VyacheslavsMegaWebApp.Data.Entities
{
    public class DownloadButton : Base.EntityBase
    {
        [Display(Name = "Надпись кнопки")]
        public string Title { get; set; }

        [Display(Name = "URL кнопки")]
        public string Url { get; set; }

        [Display(Name = "Локальный ресурс (~/)?")]
        public bool IsResourceLocal { get; set; }

        [Display(Name = "Показать кнопку скачивания")]
        public bool ShowButton { get; set; }
    }
}
