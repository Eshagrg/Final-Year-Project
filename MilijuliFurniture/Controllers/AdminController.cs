using AspNetCoreHero.ToastNotification.Abstractions;
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
    }
}
