using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuizaLabs.Challenge.Repositories.Configurations.Models
{
    public class ClientEntityTypeConfiguration : BaseEntityTypeConfiguration<Client>
    {
        protected override void Configurations(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable("clients");
            builder.Property(x => x.Name).HasColumnName("name").HasMaxLength(255).IsRequired();
            builder.Property(x => x.Email).HasColumnName("email").HasMaxLength(255).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}