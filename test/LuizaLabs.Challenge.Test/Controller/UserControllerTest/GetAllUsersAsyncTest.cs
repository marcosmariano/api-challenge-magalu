using System.Collections.Generic;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.UserControllerTest
{
    public class GetAllUsersAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetAllUsersAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_Success_ReturnsUsers()
        {
            #region Arrange

            _fixture.Initialize();
            List<User> users = new List<User>()
            {
                new User()
                {
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    Password = "cr7_melhor_do_mundo",
                    Role = "Admin",
                    Username = "cr7",
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.yKpJnp_W0wEIp2uAVK30NEc3Y-lgdHQ0CkrUnyVjfBg"
                }
            };
            _fixture.MockUserBusiness.Setup(x => x.GetAllAsync()).ReturnsAsync(users);

            #endregion

            var result = await _fixture.Controller.GetAllUsersAsync();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}