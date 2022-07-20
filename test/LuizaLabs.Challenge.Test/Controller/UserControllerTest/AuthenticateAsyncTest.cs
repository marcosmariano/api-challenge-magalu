using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.UserControllerTest
{
    public class AuthenticateAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public AuthenticateAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_ModelStateIsInvalid_ReturnsBadRequest()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.Controller.ModelState.AddModelError("error", "Bad Request");

            #endregion

            var result = await _fixture.Controller.AuthenticateAsync(new ViewModels.Users.AuthenticateViewModel());

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_Authenticate_ReturnsBadRequest()
        {
            #region Arrange

            _fixture.Initialize();
            User user = null;
            _fixture.MockUserBusiness.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

            #endregion

            var result = await _fixture.Controller.AuthenticateAsync(new ViewModels.Users.AuthenticateViewModel());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task When_Success_ReturnsUserWithToken()
        {
            #region Arrange

            _fixture.Initialize();
            User user = new User()
            {
                FirstName = "Cristiano",
                LastName = "Ronaldo",
                Password = "cr7_melhor_do_mundo",
                Role = "Admin",
                Username = "cr7",
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.yKpJnp_W0wEIp2uAVK30NEc3Y-lgdHQ0CkrUnyVjfBg"
            };
            _fixture.MockUserBusiness.Setup(x => x.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(user);

            #endregion

            var result = await _fixture.Controller.AuthenticateAsync(new ViewModels.Users.AuthenticateViewModel());

            Assert.IsType<OkObjectResult>(result);
        }
    }
}