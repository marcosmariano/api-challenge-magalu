using System;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.ClientBusinessTest
{
    public class DeleteAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public DeleteAsyncTest(Fixture fixture)
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
            var exception = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _fixture.Business.DeleteAsync(id));

            // Then
            Assert.Equal($"Parameter value must be at least 0 (Parameter 'id')\nActual value was {id}.", exception.Message);
            Assert.Equal("id", exception.ParamName);
            Assert.Equal(id, exception.ActualValue);
        }

        [Fact]
        public async Task When_Update_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.Delete(It.IsAny<int>()))
                .Throws(new Exception("Error when delete data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.DeleteAsync(1));

            // Then
            Assert.Equal("Error when delete data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository.Setup(x => x.Delete(It.IsAny<int>()));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            var result = await _fixture.Business.DeleteAsync(1);

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<Client>(It.IsAny<bool>()), Times.Once);
            _fixture.MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<bool>()), Times.Once);
            Assert.True(result);
        }
    }
}