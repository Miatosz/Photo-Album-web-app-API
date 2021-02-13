using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ImageAlbumAPI.Controllers;
using ImageAlbumAPI.Data;
using ImageAlbumAPI.Dtos.GetDtos;
using ImageAlbumAPI.Models;
using ImageAlbumAPI.Repositories;
using ImageAlbumAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace ImageAlbumAPITests.ControllersTests
{
    public class PhotoControllerTests
    {

        PhotoController _controller;
        Mock<IPhotoService> _photoService;
        Mock<FakeUserManager> _userManager;
        IMapper _mapper;
        List<Photo> Photos;
        List<User> Users;


        [SetUp]
        public void Setup()
        {
            _userManager = new FakeUserManagerBuilder()
                .Build();

            var config = new MapperConfiguration(cfg => 
            { 
                cfg.CreateMap<Photo, GetPhotoDto>();
                cfg.CreateMap<GetPhotoDto, Photo>();
            });
            _mapper = config.CreateMapper(); 

            Photos = new List<Photo>
            {
                new Photo
                {
                    Id = 1,
                    Likes = new List<Like>()
                },
                new Photo
                {
                    Id = 2,
                    Likes = new List<Like>()
                },
                new Photo
                {
                    Id = 3,
                    Likes = new List<Like>()        
                }
            };

            Users = new List<User>
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
        public void Get_WhenCalled_ReturnsOkResult()
        {
            
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.Photos).Returns(Photos);
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object);


            var result = controller.Get();


            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void Get_WhenCalled_ReturnsAllPhotos()
        {           
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.Photos).Returns(Photos);
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object); 

            var okResult = controller.Get().Result as OkObjectResult;
            var items = okResult.Value as IEnumerable<GetPhotoDto>;


            Assert.IsInstanceOf<IEnumerable<GetPhotoDto>>(okResult.Value);
            Assert.AreEqual(3, items.Count());
        }

        [Test]
        public void GetById_WhenCalled_ReturnsOkResult()
        {
            var id = 1;
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.GetPhotoById(id)).Returns(Photos.FirstOrDefault(c => c.Id == id));
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object);             

            var result = controller.GetById(id);

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void GetById_WhenCalled_ReturnsPhotoById()
        {
            var id = 1;
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.GetPhotoById(id)).Returns(Photos.FirstOrDefault(c => c.Id == id));            
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object);             

            var result = controller.GetById(id).Result as OkObjectResult;
            var item = result.Value as GetPhotoDto;

            Assert.AreEqual(id, item.Id);
            Assert.IsInstanceOf<GetPhotoDto>(result.Value);
        }

        [Test]
        public void PostPhoto_WhenCalled_AddPhotoToRepo()
        {
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.Photos).Returns(Photos);            
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object); 
            var newPhoto = new Photo
            {
                Id = 4,
                Description = "test"
            };
            Photos.Add(newPhoto);

            var result = controller.PostPhoto(newPhoto).Result as OkObjectResult;
            var photo = result.Value as Photo;

            Assert.IsInstanceOf<Photo>(result.Value);
            Assert.AreEqual(Photos.FirstOrDefault(c => c.Id == 4).Id, photo.Id);
            Photos.Remove(newPhoto);
        }

        [Test]
        public void PostPhoto_WhenCalled_WhenPhotoAdded_ReturnOkResult()
        {
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.Photos).Returns(Photos);            
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object); 
            
            
            var result = controller.PostPhoto(new Photo());

            Assert.IsInstanceOf<OkObjectResult>(result.Result);
        }

        [Test]
        public void PostPhoto_WhenCalled_WhenPhotoModelIsNotValid_ReturnBadRequest()
        {
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.Photos).Returns(Photos);            
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object); 
            var newPhoto = new Photo(){ DateOfAdd = null };

            controller.ModelState.AddModelError("Photo can't be null", "null photo");
            var result = controller.PostPhoto(newPhoto);

            Assert.IsInstanceOf<BadRequestResult>(result.Result);

        }

        // to fix
        [Test]
        public void LikePhoto_WhenCalled_ShouldAddLikeToPhotoAndReturnOkResult()
        {
            int photoId = 1;
            var mockService = new Mock<IPhotoService>();
            mockService.Setup(service => service.GetPhotoById(photoId)).Returns(Photos[photoId]);            
            // mockService.SetupAdd(service => service.LikePhoto(Photos[photoId], "1")).Raises(Photos[photoId].Likes.Add(new Like { Id = 1, UserName = "test"}));
            var fakeUserManager = new FakeUserManagerBuilder()
                .Build();            
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));
            fakeUserManager.Setup(u => u.FindByNameAsync("test1")).Returns(Task.FromResult(new User  {Id = "1", UserName = "test1" }));
            var controller = new PhotoController(mockService.Object, _mapper, fakeUserManager.Object);            
            controller.Url = mockUrlHelper.Object;
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
            controller.ControllerContext = context;


            
            var result = controller.LikePhoto(photoId);



            Assert.IsInstanceOf<OkResult>(result);
        }

        [Test]
        public void LikePhoto_WhenCalled_IfUserAllreadyLikedPhoto_ShouldReturnBadRequestResult()
        {

        }

        [Test]
        public void LikePhoto_WhenCalled_IfPhotoIsNull_ShouldReturnNotFoundResult()
        {
            int photoId = 1;
            var mockService = new Mock<IPhotoService>();
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object);  
            
            var result = controller.LikePhoto(photoId);

            Assert.IsInstanceOf<NotFoundResult>(result);       
        }

        [Test]
        public void UnlikePhoto_WhenCalled_IfPhotoIsNull_ShouldReturnNotFoundResult()
        {
            int photoId = 1;
            var mockService = new Mock<IPhotoService>();
            var controller = new PhotoController(mockService.Object, _mapper, _userManager.Object); 

            var result = controller.UnlikePhoto(photoId);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public void UnlikePhoto_WhenCalled_IfUserDidNotLikedPhoto_ShouldReturnBadRequestResult()
        {

        }

        // to fix
        [Test]
        public void UnlikePhoto_WhenCalled_ShouldRemoveLikeFromPhotoAndReturnOkResult()
        {
            int photoId = 1;
            var mockService = new Mock<IPhotoService>();
            Photos[photoId].Likes.Add(new Like { Id = 1, UserId = "1"});
            Photos[photoId].NumberOfLikes = 1;
            mockService.Setup(service => service.GetPhotoById(photoId)).Returns(Photos[photoId]);            

            var fakeUserManager = new FakeUserManagerBuilder()
                .Build();            
            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));
            fakeUserManager.Setup(u => u.FindByNameAsync("test1")).Returns(Task.FromResult(new User  {Id = "1", UserName = "test1" }));
            var controller = new PhotoController(mockService.Object, _mapper, fakeUserManager.Object);            
            controller.Url = mockUrlHelper.Object;
            var context = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
            controller.ControllerContext = context;


            
            var result = controller.UnlikePhoto(photoId);



            Assert.IsInstanceOf<OkResult>(result);
        }

       
    }


}