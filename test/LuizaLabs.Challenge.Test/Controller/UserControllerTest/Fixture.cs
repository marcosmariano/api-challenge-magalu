using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Controllers;
using Moq;

namespace LuizaLabs.Challenge.Test.Controller.UserControllerTest
{
    public class Fixture
    {
        public Mock<IUserBusiness> MockUserBusiness { get; set; }
        public UserController Controller { get; set; }

        public void Initialize()
        {
            MockUserBusiness = new Mock<IUserBusiness>();
            Controller = new UserController(MockUserBusiness.Object);
        }
    }
}