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

namespace LuizaLabs.Challenge.Test.Business.ClientBusinessTest
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
                               It.IsAny<Expression<Func<Client, bool>>>(),
                               It.IsAny<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
                               It.IsAny<Func<IQueryable<Client>, IIncludableQueryable<Client, object>>>(),
                               It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), default(CancellationToken),
                               It.IsAny<bool>())).ThrowsAsync(new Exception("Error when search data"));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var exception = await Assert.ThrowsAsync<Exception>(() => _fixture.Business.GetAllAsync());

            // Then
            Assert.Equal("Error when search data", exception.Message);
        }

        [Fact]
        public async Task When_Success_ReturnsListOfClients()
        {
            #region Arrange
            var clientList = new List<Client>()
            {
                new Client()
                {
                    Name = "Cristiano",
                    Email="cr7@hotmail.com"
                }
            };

            _fixture.Initialize();
            _fixture.MockRepository.Setup(
                            x => x.GetPagedListAsync(
                                It.IsAny<Expression<Func<Client, bool>>>(),
                                It.IsAny<Func<IQueryable<Client>, IOrderedQueryable<Client>>>(),
                                It.IsAny<Func<IQueryable<Client>, IIncludableQueryable<Client, object>>>(),
                                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<bool>(), default(CancellationToken),
                                It.IsAny<bool>())).ReturnsAsync(clientList.ToPagedList(0, 1));
            _fixture.MockUnitOfWork.Setup(x => x.GetRepository<Client>(It.IsAny<bool>())).Returns(_fixture.MockRepository.Object);
            #endregion

            // When
            var result = await _fixture.Business.GetAllAsync();

            // Then
            _fixture.MockUnitOfWork.Verify(x => x.GetRepository<Client>(It.IsAny<bool>()), Times.Once);
            Assert.Equal("cr7@hotmail.com", result.ToList()[0].Email);
            Assert.Equal("Cristiano", result.ToList()[0].Name);
        }
    }
}