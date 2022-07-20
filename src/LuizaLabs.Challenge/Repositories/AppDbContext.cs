using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore;

namespace LuizaLabs.Challenge.Repositories
{
    public class AppDbContext : DbContext
    {
        /// <summary>
        /// Construtor da classe de conexão com banco de dados
        /// </summary>
        /// <param name="options">Valores a serem enviados para a herança do DbContext</param>
        /// <returns>Instance of <see cref="AppDbContext"/></returns>
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        { }

        /// <summary>
        /// Repositório de usuários
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Repositório de clientes
        /// </summary>
        public DbSet<Client> Clients { get; set; }

        /// <summary>
        /// Modelo de implementação de criação
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}