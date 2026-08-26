using System.ComponentModel.DataAnnotations;
using VyacheslavsMegaWebApp.Models;

namespace VyacheslavsMegaWebApp.Data.Entities
{
    public class UserReport : Base.EntityBase
    {
        public UserReport() 
        {
            TimestampCreated = DateTime.Now;
        }

        [MinLength(5)]
        [MaxLength(50)]
        [Display(Name = "Тема")]
        public string Subject { get; set; }

        [MinLength(5)]
        [MaxLength(500)]
        [Display(Name = "Описание проблемы (максимум 500 символов)")]
        public string ReportText { get; set; }

        public DateTime TimestampCreated { get; private set; }
    }
}
