using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Text;

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

        //Staff Index

        public IActionResult StaffIndex()
        {
            IEnumerable<Portal_User> obj = _userAuth.GetStaffList();
            return View(obj);
        }

        public IActionResult AddStaff()
        {
            AddStaff_VM obj = new AddStaff_VM();
            return View(obj);
        }

        [HttpPost]
        public IActionResult AddStaff(AddStaff_VM obj)
        {

            AddStaff_VM vm = new AddStaff_VM();
            vm.FullName = obj.FullName;
            vm.PhoneNo = obj.PhoneNo;
            vm.Email = obj.Email;
            vm.Password = EncryptPassword(obj.Password);
            string output = _userAuth.SaveStaffData(vm);
            if (output == "SUCCESS")
            {
                return RedirectToAction("UserIndex");
            }
            return View();
        }

        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return null;
            }
            else
            {
                byte[] storePassword = Encoding.UTF8.GetBytes(password);
                string encryptedPassword = Convert.ToBase64String(storePassword);
                return encryptedPassword;
            }
        }

    }
}
