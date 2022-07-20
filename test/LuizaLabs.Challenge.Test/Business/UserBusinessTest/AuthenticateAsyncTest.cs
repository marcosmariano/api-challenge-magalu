using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Models;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.UserBusinessTest
{
    public class AuthenticateAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public AuthenticateAsyncTest(Fixture fixture)
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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.AuthenticateAsync(null, null));

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
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => _fixture.Business.AuthenticateAsync("user", null));

            // Then
            Assert.Equal("Value cannot be null. (Parameter 'password')", exception.Message);
            Assert.Equal("password", exception.ParamName);
        }

        [Fact]
        public async Task When_GetByUsernameAndPassword_ReturnsNull()
        {
            #region Arrange
            User user = null;
            _fixture.Initialize();
            _fixture.MockRepository
                           .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, true, false))
                           .ReturnsAsync(user);
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.AuthenticateAsync("user", "pass");

            // Then
            Assert.Null(result);
        }

        [Fact]
        public async Task When_Success_ReturnsUserWithToken()
        {
            #region Arrange
            User user = new User()
            {
                FirstName = "Cristiano",
                LastName = "Ronaldo",
                Password = "cr7_melhor_do_mundo",
                Role = "Admin",
                Username = "cr7"
            };
            _fixture.Initialize();
            _fixture.MockRepository
                           .Setup(x => x.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null, null, true, false))
                           .ReturnsAsync(user);
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.AuthenticateAsync("user", "pass");

            // Then
            Assert.NotNull(result.Token);
            Assert.Equal("Cristiano", result.FirstName);
            Assert.Equal("cr7", result.Username);
            Assert.Null(result.Password);
        }
    }
}