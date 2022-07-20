using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Controllers;
using Moq;

namespace LuizaLabs.Challenge.Test.Controller.ClientControllerTest
{
    public class Fixture
    {
        public Mock<IClientBusiness> MockClientBusiness { get; set; }
        public ClientController Controller { get; set; }

        public void Initialize()
        {
            MockClientBusiness = new Mock<IClientBusiness>();
            Controller = new ClientController(MockClientBusiness.Object);
        }
    }
}