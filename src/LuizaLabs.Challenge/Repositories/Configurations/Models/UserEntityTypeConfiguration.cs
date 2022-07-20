using System.Collections.Generic;
using LuizaLabs.Challenge.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LuizaLabs.Challenge.Repositories.Configurations.Models
{
    /// <summary>
    /// Classe para configuração de entidades para o banco, herdando da classe base
    /// </summary>
    public class UserEntityTypeConfiguration : BaseEntityTypeConfiguration<User>
    {
        protected override void Configurations(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("users");

            builder.Property(x => x.FirstName).HasColumnName("first_name").HasMaxLength(100).IsRequired();
            builder.Property(x => x.LastName).HasColumnName("last_name").HasMaxLength(100).IsRequired();
            builder.Property(x => x.Username).HasColumnName("username").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Password).HasColumnName("password").HasMaxLength(20).IsRequired();
            builder.Property(x => x.Role).HasColumnName("role").HasMaxLength(20).IsRequired();

            builder.HasIndex(x => x.Username).IsUnique();
            builder.Ignore(x => x.Token);

            // Seed para criar usuários no banco
            builder.HasData(new List<User>()
            {
                new User()
                {
                    Id=1,
                    FirstName="Admin",
                    LastName="Admin",
                    Password="admin",
                    Username="admin",
                    Role=Role.Admin
                },
                new User()
                {
                    Id=2,
                    FirstName="Normal",
                    LastName="User",
                    Password="user",
                    Username="user",
                    Role=Role.User
                }
            });
        }
    }
}