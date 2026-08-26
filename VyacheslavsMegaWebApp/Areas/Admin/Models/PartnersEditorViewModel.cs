using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    [Area("Admin")]
    public class PartnersEditorViewModel
    {
        List<Partner> _partners;

        public PartnersEditorViewModel(List<Partner> partners)
        {
            _partners = partners;
        }

        public List<Partner> Partners => _partners;
    }
}
