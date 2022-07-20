using System.Collections.Generic;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.ClientControllerTest
{
    public class GetAllClientsAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetAllClientsAsyncTest(Fixture fixture)
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

            var result = await _fixture.Controller.GetAllClientsAsync();

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_Success_ReturnsClients()
        {
            #region Arrange

            _fixture.Initialize();
            List<Client> clients = new List<Client>()
            {
                new Client()
                {
                   Name="Test",
                   Email="test@gmail.com"
                }
            };
            _fixture.MockClientBusiness.Setup(x => x.GetAllAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(clients);

            #endregion

            var result = await _fixture.Controller.GetAllClientsAsync();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}