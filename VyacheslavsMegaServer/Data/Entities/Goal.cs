using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VyacheslavsMegaServer.Data.Entities
{
    public class Goal : Base.EntityBase
    {
        public const string LOGOS_DIR = "goals";

        [MaxLength(50)]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [MaxLength(255)]
        [Display(Name = "Описание")]
        public string Description { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Выполнена")]
        public bool IsCompleted { get; set; }

        public DateTime Date { get; set; }

        [ValidateNever]
        [Display(Name = "Логотип")]
        public string LogoFileName { get; set; }

        [NotMapped]
        [ValidateNever]
        public string LogoFullPath => Path.Combine("img", LOGOS_DIR, LogoFileName);

        [NotMapped]
        [ValidateNever]
        public bool HasLogo => !string.IsNullOrEmpty(LogoFileName);
    }
}
