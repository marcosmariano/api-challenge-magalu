using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LuizaLabs.Challenge.Test.Controller.UserControllerTest
{
    public class GetUserByIdAsync : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetUserByIdAsync(Fixture fixture)
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

            var result = await _fixture.Controller.GetUserByIdAsync(1);

            Assert.IsType<BadRequestResult>(result);
        }
    }
}