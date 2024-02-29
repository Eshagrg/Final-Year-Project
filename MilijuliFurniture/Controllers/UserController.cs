using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace MilijuliFurniture.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserAuth _userAuth;
        private readonly IFurnitureItems _furnitureItems;
        private readonly INotyfService _toastNotificationHero;
        public UserController(IUserAuth userAuth, IFurnitureItems furnitureItems, INotyfService toastNotificationHero)
        {
            _userAuth = userAuth;
            _furnitureItems = furnitureItems;
            _toastNotificationHero = toastNotificationHero;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AccessDenied()
        {
            return View();
        }

        //Authentication and Authorization

        [HttpGet]
        public IActionResult Signup()
        {
            ViewBag.Message = null;
            Signup_VM obj = new Signup_VM();
            return View(obj);
        }


        [HttpPost]
        public IActionResult Signup(Signup_VM obj)
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

                Save_PortalUser data = new Save_PortalUser();
                data.FullName = obj.FullName;
                data.Email = obj.Email;
                data.PhoneNo = obj.PhoneNo;
                data.Password = EncryptPassword(obj.Password);
                data.Role = 1; //User Role

                string output = _userAuth.SaveUserData(data);
                if (output == "SUCCESS")
                {
                    _toastNotificationHero.Success("Signed in succesfully");
                    return RedirectToAction("Login");
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
        public IActionResult Login()
        {
            if (User!.Identity!.IsAuthenticated)
            {
                IPrincipal iPrincipalUser = User;
                var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "";
                if (userId != string.Empty)
                {
                    Portal_User usr = _userAuth.GetUserDetail(int.Parse(userId));
                    if (usr.Id > 0)
                    {
                        if (usr.RoleName == "Admin" || usr.RoleName == "Staff")
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            return RedirectToAction("Dashboard", "Home");
                        }
                    }
                }
            }
            Login_VM obj = new Login_VM();
            return View(obj);
        }


        [HttpPost]
        public async Task<ActionResult> Login(Login_VM obj)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            else
            {
                try
                {
                    Login_VM newlog = new Login_VM()
                    {
                        Email = obj.Email,
                        Password = EncryptPassword(obj.Password)
                    };
                    string msg = _userAuth.CheckLogin(newlog);
                    string depatment = "";
                    if (msg == "SUCCESS")
                    {
                        Portal_User usr = _userAuth.GetUserData(newlog);


                        if (usr.RoleName == "Admin" || usr.RoleName == "Staff")
                        {
                            depatment = "Admin";

                        }
                        else
                        {
                            depatment = "User";

                        }


                        //Creating Security Context
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, usr.Email),
                    new Claim(ClaimTypes.Name, usr.FullName),
                    new Claim("UserId", usr.Id.ToString()),
                    new Claim("Email", usr.Email),
                    new Claim("Role", usr.RoleName),
                    new Claim("Deparment", depatment),


                };
                        var identity = new ClaimsIdentity(claims, "MyCookieAuth");
                        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync("MyCookieAuth", claimsPrincipal);
                        if (usr.RoleName == "Admin" || usr.RoleName == "Staff")
                        {
                            depatment = "Admin";
                            _toastNotificationHero.Success("Logged In As Admin");
                            return RedirectToAction("Index", "Admin");
                        }
                        else
                        {
                            depatment = "User";

                        }

                        _toastNotificationHero.Success("Logged in Succesfully");
                        return RedirectToAction("Index", "User");


                    }
                    else
                    {
                        _toastNotificationHero.Error("Username and Password doesnot match or does not exist !!!");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "Could Not Connect to the Database, Check Connection !!!";
                }

            }
            return View();

        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("LogIn");

        }
    }


}
