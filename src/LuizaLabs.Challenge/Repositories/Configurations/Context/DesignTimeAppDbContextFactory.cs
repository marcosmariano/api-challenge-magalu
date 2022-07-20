using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Npgsql;

namespace LuizaLabs.Challenge.Repositories.Configurations.Context
{
    public class DesignTimeAppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        /// <summary>
        /// Método utilizado apenas para geração da migration
        /// Os dados de conexão aqui podem ser fake
        /// </summary>
        /// <returns>AppDbContext</returns>
        public AppDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppDbContext>();
            var connection = new NpgsqlConnectionStringBuilder();
            connection.Database = "test";
            builder.UseNpgsql(connection.ToString());
            return new AppDbContext(builder.Options);
        }
    }
}