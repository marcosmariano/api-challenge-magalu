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
    public class GetAllAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetAllAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_Dispose_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.Business.Dispose();
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ObjectDisposedException>(() => _fixture.Business.GetAllAsync());

            // Then
            Assert.Equal("Cannot access a disposed object.\nObject name: 'ProductBusiness'.", exception.Message);
        }


        [Fact]
        public async Task When_GetAllAsync_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockService.Setup(x => x.GetAllAsync(It.IsAny<int>())).ThrowsAsync(new Exception("Erro on call product api"));
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetAllAsync());

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
            var jsonResponse = JsonSerializer.Serialize(new ProductResponse()
            {
                Meta = new ProductResponse.MetaRecords() { PageNumber = 1, PageSize = 100 },
                Products = new System.Collections.Generic.List<Product>()
                {
                    new Product()
                    {
                         Brand = "Nike",
                        Id = id,
                        Image = "localhost/image",
                        Price = 14.99,
                        Title = "Meia"
                    }
                }
            });

            HttpContent content = new StringContent(jsonResponse.ToString());
            httpResponseMessage.Content = content;
            _fixture.MockService.Setup(x => x.GetAllAsync(It.IsAny<int>())).ReturnsAsync(httpResponseMessage);
            #endregion

            // When
            var result = await _fixture.Business.GetAllAsync();

            // Then
            _fixture.MockService.Verify(x => x.GetAllAsync(It.IsAny<int>()), Times.Once);
            Assert.Equal(id, result[0].Id);
            Assert.Equal("localhost/image", result[0].Image);
        }
    }
}