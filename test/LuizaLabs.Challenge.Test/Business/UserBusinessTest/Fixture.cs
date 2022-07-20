using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Infra.Configurations.Options;
using LuizaLabs.Challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace LuizaLabs.Challenge.Test.Business.UserBusinessTest
{
    public class Fixture
    {
        public Mock<ILogger<UserBusiness>> MockLogger { get; set; }
        public Mock<IUnitOfWork> MockUnitOfWork { get; set; }
        public Mock<IOptions<UserOptions>> MockUserOptions { get; set; }
        public Mock<IRepository<User>> MockRepository { get; set; }
        public UserBusiness Business { get; set; }

        public void Initialize()
        {
            MockLogger = new Mock<ILogger<UserBusiness>>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockUserOptions = new Mock<IOptions<UserOptions>>();
            MockRepository = new Mock<IRepository<User>>();
            MockUserOptions.Setup(x => x.Value).Returns(new UserOptions()
            {
                Secret = "Any string here to generate the jwt token"
            });

            Business = new UserBusiness(MockLogger.Object, MockUnitOfWork.Object, MockUserOptions.Object);
        }
    }
}