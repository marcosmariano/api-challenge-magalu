using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using LuizaLabs.Challenge.ViewModels.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Npgsql;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.ClientControllerTest
{
    public class CreateClientAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public CreateClientAsyncTest(Fixture fixture)
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

            var result = await _fixture.Controller.CreateClientAsync(new UpdateAndCreateViewModel());

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            var result = await _fixture.Controller.CreateClientAsync(new UpdateAndCreateViewModel() { Email = "test@gmail.com", Name = "test" });

            Assert.IsType<ObjectResult>(result);
        }

        [Fact]
        public async Task When_EmailExists_ReturnsBadRequest()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockClientBusiness.Setup(x => x.CreateAsync(It.IsAny<Client>()))
                .ThrowsAsync(new DbUpdateException("Error", new PostgresException("23505: duplicate key value violates unique constraint", "low", null, null)));

            #endregion

            var result = await _fixture.Controller.CreateClientAsync(new UpdateAndCreateViewModel());

            Assert.IsType<ObjectResult>(result);
        }
    }
}