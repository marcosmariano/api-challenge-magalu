using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.ClientControllerTest
{
    public class GetClientByIdAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetClientByIdAsyncTest(Fixture fixture)
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

            var result = await _fixture.Controller.GetClientByIdAsync(1);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_ClientIsNull_ReturnsNotFound()
        {
            #region Arrange

            _fixture.Initialize();
            Client client = null;
            _fixture.MockClientBusiness.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);

            #endregion

            var result = await _fixture.Controller.GetClientByIdAsync(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();
            Client client = new Client()
            {
                Name = "Test",
                Email = "test@gmail.com"
            };
            _fixture.MockClientBusiness.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(client);

            #endregion

            var result = await _fixture.Controller.GetClientByIdAsync(1);

            Assert.IsType<OkObjectResult>(result);
        }
    }
}