using System;
using System.Threading;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.UserBusinessTest
{
    public class CreateAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public CreateAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_UserIsNull_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.CreateAsync(null));

            // Then
            Assert.Equal("Value cannot be null. (Parameter 'user')", exception.Message);
            Assert.Equal("user", exception.ParamName);
        }

        [Fact]
        public async Task When_InsertAsync_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository.Setup(x => x.InsertAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Error when insert data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.CreateAsync(new User() { Password = $"123" }));

            // Then
            Assert.Equal("Error when insert data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsClient()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository.Setup(x => x.InsertAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);

            #endregion

            // When
            await _fixture.Business.CreateAsync(new User() { Password = $"123" });

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<User>(It.IsAny<bool>()), Times.Once);
            _fixture.MockUnitOfWork.Verify(x => x.SaveChangesAsync(It.IsAny<bool>()), Times.Once);
        }
    }
}