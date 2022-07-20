using System;
using System.Threading;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.ClientBusinessTest
{
    public class UpdateAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public UpdateAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_ClientIsNull_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.UpdateAsync(null));

            // Then
            Assert.Equal("Value cannot be null. (Parameter 'client')", exception.Message);
            Assert.Equal("client", exception.ParamName);
        }

        [Fact]
        public async Task When_Update_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.Update(It.IsAny<Client>()))
                .Throws(new Exception("Error when update data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.UpdateAsync(new Client() { Email = $"123@gmail.com" }));

            // Then
            Assert.Equal("Error when update data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository.Setup(x => x.Update(It.IsAny<Client>()));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            var result = await _fixture.Business.UpdateAsync(new Client() { Email = $"123@gmail.com" });

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<Client>(It.IsAny<bool>()), Times.Once);
            _fixture.MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<bool>()), Times.Once);
            Assert.Equal("123@gmail.com", result.Email);
        }
    }
}