using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace LuizaLabs.Challenge.Test.Business.ClientBusinessTest
{
    public class Fixture
    {
        public Mock<ILogger<ClientBusiness>> MockLogger { get; set; }
        public Mock<IUnitOfWork> MockUnitOfWork { get; set; }
        public Mock<IRepository<Client>> MockRepository { get; set; }
        public ClientBusiness Business { get; set; }

        public void Initialize()
        {
            MockLogger = new Mock<ILogger<ClientBusiness>>();
            MockUnitOfWork = new Mock<IUnitOfWork>();
            MockRepository = new Mock<IRepository<Client>>();

            Business = new ClientBusiness(MockLogger.Object, MockUnitOfWork.Object);
        }
    }
}