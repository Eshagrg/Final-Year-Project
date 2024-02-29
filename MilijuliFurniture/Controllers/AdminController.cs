using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Interface;

namespace MilijuliFurniture.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserAuth _userAuth;
        private readonly IFurnitureItems _furnitureItems;
        private readonly INotyfService _toastNotificationHero;

        public AdminController(IUserAuth userAuth, IFurnitureItems furnitureItems, INotyfService toastNotificationHero)
        {
            _userAuth = userAuth;
            _furnitureItems = furnitureItems;
            _toastNotificationHero = toastNotificationHero;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}
