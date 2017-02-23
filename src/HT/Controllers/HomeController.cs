using Microsoft.AspNetCore.Mvc;

namespace HT.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}