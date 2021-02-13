using System;
using System.Collections.Generic;
using System.Linq;
using ImageAlbumAPI.Controllers;
using ImageAlbumAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace ImageAlbumAPITests.ControllersTests
{
    public class AdminControllerTests
    {
        Mock<FakeUserManager> _userManager;
        List<User> _users;

        [SetUp]
        public void Setup()
        {
            _userManager = new FakeUserManagerBuilder()
                .Build();

            _users = new List<User>
            {
                new User
                {
                    Id = "1",
                    UserName = "test1"
                },
                new User
                {
                    Id = "2",
                    UserName = "test2"
                }
            };
        }

        [Test]
        public void GetUsers_ShouldReturnAllUsers()
        {
            var controller = new AdminController(_userManager.Object);
            _userManager.Setup(u => u.Users).Returns(_users.AsQueryable());

            var result = controller.GetUsers();

            Assert.AreEqual(result.Count(), _users.Count());
            Assert.AreEqual(result.ElementAt(0), _users.ElementAt(0));
        }

        [Test]
        public void GetUserById_IfUserExist_ShouldReturnUserById()
        {
            var id = "1";
            var controller = new AdminController(_userManager.Object);
            _userManager.Setup(u => u.Users).Returns(_users.AsQueryable());            

            var result = controller.GetUserById(id).Result as OkObjectResult;
            var user = result.Value as User;
        
            Assert.AreEqual(id, user.Id);
            Assert.IsInstanceOf<User>(result.Value);
        }

        [Test]
        public void GetUserById__IfUserExist_ShouldReturnOkResult()
        {
            var id = "1";
            var controller = new AdminController(_userManager.Object);
            _userManager.Setup(u => u.Users).Returns(_users.AsQueryable());            

            var result = controller.GetUserById(id).Result;

            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public void GetUserById__IfUserNotExist_ShouldReturnNotFoundResult()
        {
            var id = "1";
            var controller = new AdminController(_userManager.Object);
            _userManager.Setup(u => u.Users).Returns(new List<User>().AsQueryable());  

            var result = controller.GetUserById(id).Result;

            Assert.IsInstanceOf<NotFoundResult>(result);
        }
    }
}