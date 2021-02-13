// using System.Collections.Generic;
// using ImageAlbumAPI.Data;
// using ImageAlbumAPI.Models;
// using ImageAlbumAPI.Repositories;
// using ImageAlbumAPI.Services;
// using Microsoft.EntityFrameworkCore;
// using Moq;
// using NUnit.Framework;

// namespace ImageAlbumAPITests.ServicesTests
// {
//     public class PhotoServiceTest
//     {
//         Mock<IPhotoRepo> _photoRepo;
//         Mock<IUserRepo> _userRepo;
//         Mock<PhotoService> photoService;
//         Mock<AppDbContext> MockContext;
        
//         [SetUp]
//         public void SetUp()
//         {
//             _photoRepo = new Mock<IPhotoRepo>();
//             _userRepo = new Mock<IUserRepo>();

//             var options = new DbContextOptionsBuilder<AppDbContext>()
//                 .UseInMemoryDatabase(databaseName: "ImageAlbum")
//                 .Options;
            
//             MockContext = new Mock<AppDbContext>(options);
//             photoService = new Mock<PhotoService>(MockContext.Object, _photoRepo.Object, _userRepo.Object);
//         }

//         [Test]
//         public void LikePhoto_ShouldIncreaseNumberOfLikesAndAddUserToList()
//         {
//             var photo = new Photo(){Likes = new List<Like>()};
//             var user = new User() {Id = "0", UserName = "test"};
//             var like = new Like{Id = 0, UserName = "test", UserId = "0", User = user};
//             photoService.Object.LikePhoto(photo, "0");

//             Assert.Equals(photo.NumberOfLikes, 1);
//             Assert.Contains(like, photo.Likes);
//         }

        
//     }
// }