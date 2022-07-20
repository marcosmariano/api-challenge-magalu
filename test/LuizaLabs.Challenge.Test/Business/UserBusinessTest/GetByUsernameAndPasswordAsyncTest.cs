using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.UserBusinessTest
{
    public class GetByUsernameAndPasswordAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetByUsernameAndPasswordAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_UsernameIsNull_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.GetByUsernameAndPasswordAsync(null, null));

            // Then
            Assert.Equal("Value cannot be null. (Parameter 'username')", exception.Message);
            Assert.Equal("username", exception.ParamName);
        }

        [Fact]
        public async Task When_PasswordIsNull_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();

            #endregion

            // When
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.GetByUsernameAndPasswordAsync("user", null));

            // Then
            Assert.Equal("Value cannot be null. (Parameter 'password')", exception.Message);
            Assert.Equal("password", exception.ParamName);
        }

        [Fact]
        public async Task When_GetFirstOrDefault_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, true, false))
                .ThrowsAsync(new Exception("Error when search data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetByUsernameAndPasswordAsync("user", "pass"));

            // Then
            Assert.Equal("Error when search data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsUser()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository
                .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, true, false))
                .ReturnsAsync(new User() { Password = "123", Username = "Rambo" });
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.GetByUsernameAndPasswordAsync("user", "pass");

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<User>(It.IsAny<bool>()), Times.Once);
            _fixture.MockRepository.Verify(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, true, false), Times.Once);
            Assert.Equal("Rambo", result.Username);
            Assert.Null(result.Password);
        }
    }
}