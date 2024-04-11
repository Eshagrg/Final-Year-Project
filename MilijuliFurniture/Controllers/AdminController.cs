﻿using AspNetCoreHero.ToastNotification.Abstractions;
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
        public async Task<IActionResult> GetSummary()
        {
            try
            {
                VMDashBoard vmDashboard = new VMDashBoard();

                // Populate the view model with data from the dashboard service
                vmDashboard.TotalSales = await _furnitureItems.TotalSalesLastWeek();
                //vmDashboard.TotalIncome = await _furnitureItems.TotalIncomeLastWeek();
                vmDashboard.TotalProducts = await _furnitureItems.TotalProducts();
                vmDashboard.TotalCategories = await _furnitureItems.TotalCategories();
                vmDashboard.TotalUsers = await _furnitureItems.TotalUsers();
                



                // Create lists for sales and products data
                List<VMSalesWeek> listSalesWeek = new List<VMSalesWeek>();
                List<VMProductsWeek> ProductListWeek = new List<VMProductsWeek>();

                foreach (KeyValuePair<string, int> item in await _furnitureItems.SalesLastWeek())
                {
                    listSalesWeek.Add(new VMSalesWeek()
                    {
                        Date = item.Key,
                        Total = item.Value
                    });
                }

                foreach (KeyValuePair<string, int> item in await _furnitureItems.ProductsTopLastWeek())
                {
                    ProductListWeek.Add(new VMProductsWeek()
                    {
                        Product = item.Key,
                        Quantity = item.Value
                    });
                }
                // Assign the sales and products lists to the view model
                vmDashboard.SalesLastWeek = listSalesWeek;
                vmDashboard.ProductsTopLastWeek = ProductListWeek;
              

                return StatusCode(StatusCodes.Status200OK, new { Success = true, vmDashboard = vmDashboard });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
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
            if (ModelState.IsValid)
            {
                string msg = _userAuth.CheckEmailExist(obj.Email);
                if (msg == "SUCCESS")
                {
                    _toastNotificationHero.Information("Email aready exists");
                    return View();
                }

                if (obj.Password != obj.ConfirmPassword)
                {
                    _toastNotificationHero.Information("Password does not match");
                    return View();
                }
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
            }
            else
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _toastNotificationHero.Error(error.ErrorMessage);
                }
                return View(obj);
            }
            return View();
        }

        public IActionResult UpdateStaff(int id)
        {
            AddStaff_VM obj = _userAuth.GetStaffDetailsById(id);
            return View(obj);
        }

        [HttpPost]
        public IActionResult UpdateStaff(AddStaff_VM obj)
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

                Portal_User vm = new Portal_User();
                vm.Id = obj.Id;
                vm.FullName = obj.FullName;
                vm.PhoneNo = obj.PhoneNo;
                vm.Email = obj.Email;
                vm.UploadImage = fileurl;
                string output = _userAuth.UpdateStaffDetail(vm);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("StaffIndex");
                }
            }
            else
            {
                // Fetch existing user details
                AddStaff_VM existingUser = _userAuth.GetStaffDetailsById(obj.Id); // You need to implement a method to fetch user details by ID

                // Update user's details without changing the image
                Portal_User vm = new Portal_User();
                vm.Id = obj.Id;
                vm.FullName = obj.FullName;
                vm.PhoneNo = obj.PhoneNo;
                vm.Email = obj.Email;
                vm.UploadImage = existingUser.UploadImageString; // Assign the previous image URL
                string output = _userAuth.UpdateStaffDetail(vm);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("StaffIndex");
                }
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


        //Category CRUD Operations

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
            if (output == "HasValue")
            {
                _toastNotificationHero.Error("Category cannot be deleted as Product is assigned");
                return RedirectToAction("CategoryIndex");
            }
            return RedirectToAction("CategoryIndex");
        }


        //Product CRUD operations

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
                vm.Brand = obj.Brand;
                vm.Quantity = obj.Quantity;
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
                vm.Quantity = obj.Quantity;
                vm.Brand = obj.Brand;
                string creadtedBy = User.Identity.Name;
                output = _furnitureItems.SaveProductData(vm,creadtedBy);
                if (output == "SUCCESS")
                {
                    return RedirectToAction("ProductIndex");
                }

            }
            return View();
        }

        public IActionResult UpdateProduct(int id)
        {
            List<Category> categories = _furnitureItems.GetCategorylist().ToList();
            categories.Insert(0, new Category { Id = 0, Name = "Select Category" });

            ViewBag.ListOfCategory = categories;

            Product obj = _furnitureItems.GetProductDetailsById(id);
            ViewBag.SelectedCategoryId = obj.CategoryId;
            return View(obj);

          
        }

        [HttpPost]
        public IActionResult UpdateProduct(Product obj, int id)
        {
            string output = _furnitureItems.UpdateProductDetail(obj,id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("ProductIndex");
            }
            return View();
        }

        public IActionResult DeleteProduct(int id)
        {
            string output = _furnitureItems.DeleteProductDetail(id);
            if (output == "SUCCESS")
            {
                return RedirectToAction("ProductIndex");
            }
            return RedirectToAction("ProductIndex");
        }

    }

}
