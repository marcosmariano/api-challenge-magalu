using LuizaLabs.Challenge.Business;
using Microsoft.Extensions.DependencyInjection;

namespace LuizaLabs.Challenge.Extensions
{
    /// <summary>
    /// Classe responsável por adicionar as Business a injeção de dependencia
    /// </summary>
    public static class BusinessExtensions
    {
        /// <summary>
        /// Método estático para adicionar as Business
        /// </summary>
        /// <param name="services">Services vindo do Startup</param>
        public static void AddBusiness(this IServiceCollection services)
        {
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<IProductBusiness, ProductBusiness>();
            services.AddTransient<IClientBusiness, ClientBusiness>();
        }
    }
}