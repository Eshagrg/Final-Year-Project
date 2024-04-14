using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilijuliFurniture.Controllers;
using Moq;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MilijuliFurnitureUnitTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public async Task Login_ValidCredentials_RedirectToAction()
        {
            // Arrange
            var userAuthMock = new Mock<IUserAuth>();
            var toastNotificationMock = new Mock<INotyfService>();

            // Mock CheckLogin to return "SUCCESS"
            userAuthMock.Setup(mock => mock.CheckLogin(It.IsAny<Login_VM>())).Returns("SUCCESS");

            // Mock GetUserData to return a user with RoleName "User"
            userAuthMock.Setup(mock => mock.GetUserData(It.IsAny<Login_VM>())).Returns(new Portal_User { RoleName = "Admin" });

            var controller = new UserController(userAuthMock.Object, null, toastNotificationMock.Object);
            var loginModel = new Login_VM { Email = "admin@gmail.com", Password = "Esha@12345" };

            // Act
            var result = await controller.Login(loginModel) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Home", result.ControllerName);
        }
    }
}
