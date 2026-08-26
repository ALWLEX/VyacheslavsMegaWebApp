using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace VyacheslavsMegaWebApp.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult QR()
        {
            return File("~/img/siteQR.png", "image/png");
        }

        public IActionResult Wallpaper() 
        {
            return File("~img/win10wallpaper.jpg", "image/jpeg");
        }
    }
}
