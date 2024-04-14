using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Moq;
using Site.DataAccess.Domain;
using Site.DataAccess.Interface;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MilijuliFurniture.Controllers;

namespace MMilijuliFurnitureUnitTest
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserAuth> _userAuthMock;
        private Mock<SignInManager<Portal_User>> _signInManagerMock;
        private UserController _controller;

        [TestInitialize]
        public void TestInitialize()
        {
            _userAuthMock = new Mock<IUserAuth>();
            _signInManagerMock = new Mock<SignInManager<Portal_User>>(
                _userAuthMock.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<Portal_User>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Portal_User>>>().Object,
                new Mock<IAuthenticationSchemeProvider>().Object);

            _controller = new UserController(_userAuthMock.Object, _signInManagerMock.Object);
        }

        [TestMethod]
        public async Task Login_ValidCredentials_ReturnsRedirectToActionResult()
        {
            // Arrange
            var loginVm = new Login_VM
            {
                Email = "admin@gmail.com",
                Password = "Esha@12345"
            };

            _userAuthMock.Setup(ua => ua.CheckLogin(loginVm)).Returns("SUCCESS");
            _userAuthMock.Setup(ua => ua.GetUserData(loginVm)).Returns(new Portal_User { Id = 1, RoleName = "Admin" });

            // Act
            var result = await _controller.Login(loginVm) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Admin", result.ControllerName);
        }

        
    }
}