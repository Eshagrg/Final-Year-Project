using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MilijuliFurniture.Models;
using System.Diagnostics;

namespace MilijuliFurniture.Controllers
{
	public class HomeController : Controller
	{
        private readonly IStringLocalizer<HomeController>? _localizer;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IStringLocalizer<HomeController>? localizer, ILogger<HomeController> logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

  
        [HttpPost]
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }

        public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
