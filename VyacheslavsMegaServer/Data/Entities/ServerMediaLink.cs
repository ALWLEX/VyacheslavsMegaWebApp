using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaServer.Data.Entities.Base;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class ServerMediaLink : Link
    {
        public const string LOGOS_DIR = "linkIcons";

        [Display(Name = "Показывать ссылку")]
        public bool ShowLink { get; set; }

        [ValidateNever]
        [Display(Name = "Логотип соц. сети")]
        public string LogoFileName { get; set; }

        [NotMapped]
        [ValidateNever]
        public string LogoFullPath => Path.Combine("uploaded-resources/img", LOGOS_DIR, LogoFileName);

        [NotMapped]
        [ValidateNever]
        public bool HasLogo => !string.IsNullOrEmpty(LogoFileName);
    }
}
