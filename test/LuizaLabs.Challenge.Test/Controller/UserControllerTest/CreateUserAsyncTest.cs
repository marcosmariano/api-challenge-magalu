using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using LuizaLabs.Challenge.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.UserControllerTest
{
    public class CreateUserAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public CreateUserAsyncTest(Fixture fixture)
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

            var result = await _fixture.Controller.CreateUserAsync(new CreateViewModel());

            Assert.IsType<BadRequestResult>(result);
        }


        [Fact]
        public async Task When_Success_ReturnsUser()
        {
            #region Arrange

            _fixture.Initialize();
            CreateViewModel createViewModel = new CreateViewModel()
            {
                FirstName = "Cristiano",
                LastName = "Ronaldo",
                Password = "cr7_melhor_do_mundo",
                Role = "Admin",
                Username = "cr7"
            };
            _fixture.MockUserBusiness
                .Setup(x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(new User()
                {
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    Password = "cr7_melhor_do_mundo",
                    Role = "Admin",
                    Username = "cr7"
                });

            #endregion

            var result = await _fixture.Controller.CreateUserAsync(createViewModel);

            Assert.NotNull(result);
        }
    }
}