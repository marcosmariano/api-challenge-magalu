using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuizaLabs.Challenge.Repositories.Configurations.Models
{
    /// <summary>
    /// Classe base para a configuração das entidades onde contem mapeamento em comum entre entidades
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade que será configurada</typeparam>
    public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IModelBase
    {
        public void Configure(EntityTypeBuilder<TEntity> builder)
        {
            ConfigurePrimaryKey(builder);
            ConfigureCreateUpdateTime(builder);
            Configurations(builder);
        }

        /// <summary>
        /// Configura a chave primária e o seu nome.
        /// </summary>
        /// <param name="builder">Builder de configuração</param>
        protected virtual void ConfigurePrimaryKey(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id").IsRequired();
        }

        /// <summary>
        /// Configura os campos de data e hora de criação e última atualização da entidade
        /// </summary>
        /// <param name="builder">Builder de configuração</param>
        protected virtual void ConfigureCreateUpdateTime(EntityTypeBuilder<TEntity> builder)
        {
            builder.Property(x => x.CreatedAt).HasColumnName("created_at").IsRequired().ValueGeneratedOnAdd();
            builder.Property(x => x.UpdatedAt).HasColumnName("updated_at").IsRequired().ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("now()");
        }

        /// <summary>
        /// Configuração da entidade
        /// </summary>
        /// <param name="builder">Builder de configuração</param>
        protected abstract void Configurations(EntityTypeBuilder<TEntity> builder);
    }
}