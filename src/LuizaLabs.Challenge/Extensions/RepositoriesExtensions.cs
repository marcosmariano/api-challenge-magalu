using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LuizaLabs.Challenge.Extensions
{
    /// <summary>
    /// Manipulação de repositórios de conexão com banco
    /// </summary>
    public static class RepositoriesExtensions
    {
        /// <summary>
        /// Método para criar os repositórios e ser utilizado na injeção de dependencias
        /// </summary>
        /// <param name="services">Services da injeção de dependencias</param>
        /// <param name="configuration">Objeto de configuração de variaveis de ambiente</param>
        public static void AddRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContextPool<AppDbContext>(options =>
                options
                    .UseNpgsql(configuration.GetNpgsqlConnectionString("test"))
                    .EnableDetailedErrors());

            services.AddUnitOfWork<AppDbContext>();
        }
    }
}