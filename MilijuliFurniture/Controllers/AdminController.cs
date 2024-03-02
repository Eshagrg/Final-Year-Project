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
            return View(new AddStaff_VM());
        }

            public IActionResult ViewUserDetail(int id)
        {
            Portal_User obj = _userAuth.GetUserDetail(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult AddStaff(AddStaff_VM obj)
        {
            if (obj.UploadImage != null)
            {
                //Upload File
                string folder = "wwwroot/uploadfiles/";
                string fileurl = "/uploadfiles/";
                string guid = Guid.NewGuid().ToString();
                fileurl += guid + obj.UploadImage.FileName;
                folder += guid + obj.UploadImage.FileName;
                string serverFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);

                obj.UploadImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                AddStaff vm = new AddStaff();
                vm.FullName = obj.FullName;
                vm.PhoneNo = obj.PhoneNo;
                vm.Email = obj.Email;
                vm.Password = EncryptPassword(obj.Password);
                vm.UploadImage = fileurl;
                string output = _userAuth.SaveStaffData(vm);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("StaffIndex");
                }
            }
            else
            {
                string folder = "wwwroot/uploadfiles/";
                string fileurl = "/uploadfiles/";
                string guid = Guid.NewGuid().ToString();
                fileurl += guid + "staff.png";
                folder += guid + "staff.png";
                string serverFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);

                //obj.UploadImage.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

                AddStaff vm = new AddStaff();
                vm.FullName = obj.FullName;
                vm.PhoneNo = obj.PhoneNo;
                vm.Email = obj.Email;
                vm.Password = EncryptPassword(obj.Password);
                vm.UploadImage = fileurl;
                string output = _userAuth.SaveStaffData(vm);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("StaffIndex");
                }
            }
            return View();
        }

        public IActionResult UpdateStaff(int id)
        {
            Portal_User obj = _userAuth.GetStaffDetailsById(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult UpdateStaff(Portal_User obj)
        {
            string output = _userAuth.UpdateStaffDetail(obj);
            if (output == "SUCCESS")
            {
                return RedirectToAction("StaffIndex");
            }
            return View();
        }

        public IActionResult VerifyUserDetail(int id)
        {
            string output = _userAuth.VerifyUserDetail(id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("StaffIndex");
            }
            return View();
        }

        public IActionResult DisableUserDetail(int id)
        {
            string output = _userAuth.DisableUserDetail(id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("StaffIndex");
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
