using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork.Collections;
using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Xunit;

namespace LuizaLabs.Challenge.Test.Business.UserBusinessTest
{
    public class GetAllAsyncTest : IClassFixture<Fixture>
    {
        private Fixture _fixture;
        public GetAllAsyncTest(Fixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task When_GetPagedListAsync_ThrowsException()
        {
            #region Arrange

            _fixture.Initialize();
            _fixture.MockRepository.Setup(
                           x => x.GetPagedListAsync(
                               It.IsAny<Expression<Func<User, bool>>>(),
                               It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                               It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                               It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), default(CancellationToken),
                               It.IsAny<bool>())).ThrowsAsync(new Exception("Error when search data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetAllAsync());

            // Then
            Assert.Equal("Error when search data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsListOfUsers()
        {
            #region Arrange
            var userList = new List<User>()
            {
                new User()
                {
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    Password = "cr7_melhor_do_mundo",
                    Role= "Admin",
                    Username="cr7"
                }
            };

            _fixture.Initialize();
            _fixture.MockRepository.Setup(
                            x => x.GetPagedListAsync(
                                It.IsAny<Expression<Func<User, bool>>>(),
                                It.IsAny<Func<IQueryable<User>, IOrderedQueryable<User>>>(),
                                It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>(),
                                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), default(CancellationToken),
                                It.IsAny<bool>())).ReturnsAsync(userList.ToPagedList(0, 1));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<User>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.GetAllAsync();

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<User>(It.IsAny<bool>()), Times.Once);
            Assert.Equal("cr7", result.ToList()[0].Username);
            Assert.Equal("Cristiano", result.ToList()[0].FirstName);
            Assert.Equal("Ronaldo", result.ToList()[0].LastName);
            Assert.Null(result.ToList()[0].Password);
        }
    }
}