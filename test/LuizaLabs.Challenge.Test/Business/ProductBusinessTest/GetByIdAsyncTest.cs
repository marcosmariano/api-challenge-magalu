using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.ProductBusinessTest
{
    public class GetByIdAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetByIdAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_GuidInvalid_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentException>(() => _fixture.Business.GetByIdAsync(Guid.Empty));

            // Then
            Assert.Equal("Parameter Invalid (Parameter 'id')", exception.Message);
        }

        [Fact]
        public async Task When_GetByIdAsync_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ThrowsAsync(new Exception("Erro on call product api"));
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetByIdAsync(Guid.NewGuid()));

            // Then
            Assert.Equal("Erro on call product api", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsProduct()
        {
            #region Arrange

            _fixture.Initialize();
            var id = Guid.NewGuid();
            var httpResponseMessage = new HttpResponseMessage();
            httpResponseMessage.StatusCode = HttpStatusCode.OK;
            var jsonResponse = JsonSerializer.Serialize(new Product()
            {
                Brand = "Nike",
                Id = id,
                Image = "localhost/image",
                Price = 14.99,
                Title = "Meia"
            });

            HttpContent content = new StringContent(jsonResponse.ToString());
            httpResponseMessage.Content = content;
            _fixture.MockService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(httpResponseMessage);
            #endregion

            // When
            var result = await _fixture.Business.GetByIdAsync(id);

            // Then
            _fixture.MockService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            Assert.Equal(id, result.Id);
            Assert.Equal("Nike", result.Brand);
        }
    }
}