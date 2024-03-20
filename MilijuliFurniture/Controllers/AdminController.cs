using AspNetCoreHero.ToastNotification.Abstractions;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Linq;
using System.Text;

namespace MilijuliFurniture.Controllers
{
    [Authorize(Policy = "MustBelongToAdminStaff")]
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
                string output = "";

                if (obj.UploadImage != null && obj.UploadImage.Length > 0)
                {
                    // If obj.UploadImage is not null and has data, proceed with uploading the image
                    fileurl += guid + "staff.png";
                    folder += guid + "staff.png";
                    string serverFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);

                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        obj.UploadImage.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    // If obj.UploadImage is null or empty, upload a default image instead
                    fileurl += "staff.png";
                }

                AddStaff vm = new AddStaff();
                vm.FullName = obj.FullName;
                vm.PhoneNo = obj.PhoneNo;
                vm.Email = obj.Email;
                vm.Password = EncryptPassword(obj.Password);
                vm.UploadImage = fileurl;

                output = _userAuth.SaveStaffData(vm);

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

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }
        //Change Password Feature

        [HttpPost]
        public IActionResult ChangePassword(int userId, string newPassword, string confirmNewPassword)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Retrieve the user's hashed password from the database using the userId
                string hashedPassword = _userAuth.GetUserHashedPassword(userId);

                // Validate the new password and confirm new password
                if (newPassword == confirmNewPassword)
                {
                    // Hash the new password before storing it in the database
                    string newHashedPassword = EncryptPassword(newPassword);

                    // Call the repository method to update the user's password
                    bool success = _userAuth.UpdateUserPassword(userId, newHashedPassword);

                    if (success)
                    {
                        _toastNotificationHero.Success("Pssword Succesfully Changed");
                        // Password changed successfully
                        return Json(new { success = true, message = "Password changed successfully." });
                    }
                    else
                    {
                        _toastNotificationHero.Error("Failed to change password. Please try again.");
                        return Json(new { success = false, message = "Failed to change password. Please try again." });
                    }
                }
                else
                {
                    _toastNotificationHero.Information("New password and confirm password do not match.");
                    return Json(new { success = false, message = "New password and confirm password do not match." });
                }
            }
            else
            {
                // Unauthorized access or not an admin
                return Json(new { success = false, message = "Unauthorized access." });
            }
        }


        //Category and Product

        public IActionResult CategoryIndex()
        {
            IEnumerable<Category> obj = _furnitureItems.GetCategorylist();
            return View(obj);
        }

        [HttpPost]
        public IActionResult AddCategory(string categoryname)
        {
            if(categoryname != "")
            {
                string creadtedBy = User.Identity.Name;
                bool success = _furnitureItems.AddCategory(categoryname,creadtedBy);
                if (success)
                {
                    _toastNotificationHero.Success("Category Added Succesfully");
                    // Password changed successfully
                    return Json(new { success = true, message = "Password changed successfully." });
                }
                else
                {
                    _toastNotificationHero.Error("Failed to change password. Please try again.");
                    return Json(new { success = false, message = "Failed to change password. Please try again." });
                }
            }
            else
            {

                _toastNotificationHero.Error("Failed to add category. Please try again.");
                return Json(new { success = false, message = "Failed to change password. Please try again." });
            }
         
        }


        [HttpPost]
        public IActionResult UpdateCategory(string obj,int id)
        {
            string output = _furnitureItems.UpdateCategoryDetail(obj,id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("CategoryIndex");
            }
            return View();
        }

        public IActionResult DeleteCategory(int id)
        {
            string output = _furnitureItems.DeleteCategoryDetail(id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("CategoryIndex");
            }
            return RedirectToAction("CategoryIndex");
        }

        public IActionResult ProductIndex()
        {
            IEnumerable<Product> obj = _furnitureItems.GetProductlist();
            return View(obj);
        }
        public IActionResult AddProduct()
        {
            // Retrieve categories from the database
            List<Category> categories = new List<Category>();
            categories = _furnitureItems.GetCategorylist().ToList();

            categories.Insert(0, new Category { Id = 0, Name = "Select Category" });

            // Pass categories to the view
            ViewBag.ListOfCategory = categories;
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product_VM obj)
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

                Product vm = new Product();
                vm.Name = obj.Name;
                vm.Price = obj.Price;
                vm.CategoryId = obj.CategoryId;
                vm.UploadImage = fileurl;
                string creadtedBy = User.Identity.Name;
                string output = _furnitureItems.SaveProductData(vm,creadtedBy);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("ProductIndex");
                }
            }
            else
            {

                string folder = "wwwroot/uploadfiles/";
                string fileurl = "/uploadfiles/";
                string guid = Guid.NewGuid().ToString();
                string output = "";

                if (obj.UploadImage != null && obj.UploadImage.Length > 0)
                {
                    // If obj.UploadImage is not null and has data, proceed with uploading the image
                    fileurl += guid + "staff.png";
                    folder += guid + "staff.png";
                    string serverFolder = Path.Combine(Directory.GetCurrentDirectory(), folder);

                    using (var fileStream = new FileStream(serverFolder, FileMode.Create))
                    {
                        obj.UploadImage.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    // If obj.UploadImage is null or empty, upload a default image instead
                    fileurl += "staff.png";
                }

                Product vm = new Product();
                vm.Name = obj.Name;
                vm.Price = obj.Price;
                vm.CategoryId = obj.CategoryId;
                vm.UploadImage = fileurl;
                string creadtedBy = User.Identity.Name;
                output = _furnitureItems.SaveProductData(vm,creadtedBy);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("ProductIndex");
                }

            }
            return View();
        }
    }
}
