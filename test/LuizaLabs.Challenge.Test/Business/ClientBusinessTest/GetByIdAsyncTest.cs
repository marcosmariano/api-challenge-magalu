using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.ClientBusinessTest
{
    public class GetByIdAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetByIdAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        /// <summary>
        /// Teste de teoria, onde são passado valores para o método
        /// E os testes são feitos encima destes valores
        /// </summary>
        /// <param name="id">valor recebido do InlineData</param>
        [Theory]
        [InlineData(-1)]
        [InlineData(-2)]
        public async Task When_IdIsOutOfRange_ThrowsException(int id)
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _fixture.Business.GetByIdAsync(id));

            // Then
            Assert.Equal($"Parameter value must be at least 0 (Parameter 'id')\nActual value was {id}.", exception.Message);
            Assert.Equal("id", exception.ParamName);
            Assert.Equal(id, exception.ActualValue);
        }

        [Fact]
        public async Task When_GetFirstOrDefaultAsync_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), null, null, true, false))
                .ThrowsAsync(new Exception("Error when search data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetByIdAsync(1));

            // Then
            Assert.Equal("Error when search data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), null, null, true, false))
                .ReturnsAsync(new Client() { Email = "123@gmail.com", Name = "Rambo" });
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.GetByIdAsync(1);

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<Client>(It.IsAny<bool>()), Times.Once);
            _fixture.MockRepository.Verify(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<Client, bool>>>(), null, null, true, false), Times.Once);
            Assert.Equal("Rambo", result.Name);
            Assert.Equal("123@gmail.com", result.Email);
        }
    }
}