using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AspNetCoreHero.ToastNotification.Abstractions;
using MilijuliFurniture.Controllers;
using Moq;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Threading.Tasks;

namespace MilijuliFurnitureUnitTest
{
    [TestClass]
    public class LoginTest
    {
        [TestMethod]
        public async Task Login_ValidCredentials_RedirectsToCorrectAction()
        {
            // Arrange
            var loginVm = new Login_VM { Email = "admin@gmail.com", Password = "Esha@1234" };

            // Mock IUserAuth
            var mockUserAuth = new Mock<IUserAuth>();
            mockUserAuth.Setup(u => u.CheckLogin(It.IsAny<Login_VM>())).Returns("SUCCESS");
            mockUserAuth.Setup(u => u.GetUserData(It.IsAny<Login_VM>())).Returns(new Portal_User { RoleName = "Admin" });

            // Mock INotyfService
            var mockToastNotificationHero = new Mock<INotyfService>();

            // Create UserController with mocked dependencies
            var controller = new UserController(mockUserAuth.Object, Mock.Of<IFurnitureItems>(), mockToastNotificationHero.Object);

            // Act
            var result = await controller.Login(loginVm) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Admin", result.ControllerName);

            int defaultDurationInSeconds = 5; // Example duration of 5 seconds
            mockToastNotificationHero.Verify(t => t.Success(It.IsAny<string>(), defaultDurationInSeconds), Times.Once);

        }

        // You can write more test methods to cover other scenarios such as invalid credentials, exceptions, etc.
    }
}
