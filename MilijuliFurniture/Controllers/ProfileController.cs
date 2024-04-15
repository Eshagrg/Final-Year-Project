using Microsoft.AspNetCore.Mvc;

namespace MilijuliFurniture.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
