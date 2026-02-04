using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Models;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class MainPageData : Base.PageData
    {
        [MaxLength(80)]
        [Display(Name = "Заголовок")]
        public string Title { get; set; }

        [MaxLength(100)]
        [Display(Name = "Текст ошибки")]
        public string ErrorMessage { get; set; }

        [Display(Name = "Показать текст ошибки")]
        public bool ShowErrorMessage { get; set; }

        [Display(Name = "Надпись кнопки")]
        public string DownloadButtonTitle { get; set; }

        [Display(Name = "URL кнопки")]
        public string DownloadButtonUrl { get; set; }

        [Display(Name = "Локальный ресурс (~/)?")]
        public bool DownloadLocalResource { get; set; }

        [Display(Name = "Показать кнопку скачивания")]
        public bool ShowDownloadButton { get; set; }

        [Display(Name = "Адрес сервера")]
        public string ServerAddress { get; set; }

        [Display(Name = "Версия сервера")]
        public string ServerVersion { get; set; }

        [MaxLength(30)]
        [Display(Name = "Жёлтая надпись")]
        public string YellowHint { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Описание проекта")]
        public string Description { get; set; }
    }
}
