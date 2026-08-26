using Microsoft.AspNetCore.Mvc;
using VyacheslavsMegaWebApp.Data.Entities;
using VyacheslavsMegaWebApp.Data.Entities.Base;

namespace VyacheslavsMegaWebApp.Areas.Admin.Models
{
    [Area("Admin")]
    public class LinksEditorViewModel<T> where T : Link
    {
        List<T> _links;

        public LinksEditorViewModel(List<T> links)
        {
            _links = links;
        }

        public List<T> Links => _links;
    }
}
