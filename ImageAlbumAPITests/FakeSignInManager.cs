// using ImageAlbumAPI.Models;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
// using Moq;

// namespace ImageAlbumAPITests
// {
//     public class FakeSignInManager : SignInManager<User>
//     {
//         public FakeSignInManager()
//             : base(new Mock<FakeUserManager>().Object,
//                    new HttpContextAccessor(),
//                    new Mock<IUserClaimsPrincipalFactory<User>>().Object,
//                    new Mock<IOptions<IdentityOptions>>().Object,
//                    new Mock<ILogger<SignInManager<User>>>().Object,
//                    new Mock<IAuthenticationSchemeProviderovider>().Object )
//         { }
//     }
// }