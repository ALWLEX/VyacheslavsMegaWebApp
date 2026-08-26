using System.ComponentModel.DataAnnotations;

namespace VyacheslavsMegaWebApp.Data.Entities.Base
{
    public abstract class PageData : EntityBase
    {
        [Display(Name = "Заголовок страницы на вкладке браузера")]
        public string PageTitle { get; set; }

        [Display(Name = "Метатэг description")]
        public string MetatagDescription { get; set; }

        [Display(Name = "Метатэг keywords")]
        public string MetatagKeywords { get; set; }
    }
}
