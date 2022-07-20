using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace LuizaLabs.Challenge.Test.Business.ProductBusinessTest
{
    public class Fixture
    {
        public Mock<ILogger<ProductBusiness>> MockLogger { get; set; }
        public Mock<IProductApiService> MockService { get; set; }
        public ProductBusiness Business { get; set; }

        public void Initialize()
        {
            MockLogger = new Mock<ILogger<ProductBusiness>>();
            MockService = new Mock<IProductApiService>();

            Business = new ProductBusiness(MockLogger.Object, MockService.Object);
        }
    }
}