using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VyacheslavsMegaServer.Data.Entities.Base;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class Partner : EntityBase
    {
        public const string LOGOS_DIR = "partners";

        [MaxLength(50)]
        [Display(Name = "Название партнёра")]
        public string Name { get; set; }

        [MaxLength(50)]
        [Display(Name = "Подзаголовок")]
        public string Subtitle { get; set; }

        [MaxLength(300)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Display(Name = "Показывать партнёра")]
        public bool ShowPartner { get; set; }

        [ValidateNever]
        [Display(Name = "Логотип")]
        public string LogoFileName { get; set; }

        [NotMapped]
        [ValidateNever]
        public string LogoFullPath => Path.Combine("img", LOGOS_DIR, LogoFileName);

        [NotMapped]
        [ValidateNever]
        public bool HasLogo => !string.IsNullOrEmpty(LogoFileName);

        #region
        [ValidateNever]
        public virtual ICollection<PartnerLink> Links { get; set; }
        #endregion

    }
}
