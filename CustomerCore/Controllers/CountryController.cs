using Microsoft.AspNetCore.Mvc;

namespace CustomerCore.Controllers
{
    public class CountryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
