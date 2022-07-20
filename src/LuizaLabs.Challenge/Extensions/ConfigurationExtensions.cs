using System;
using System.Text;
using LuizaLabs.Challenge.Infra.Configurations.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LuizaLabs.Challenge.Extensions
{
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Retorna a string de conexão com o banco de dados PostgreSQL
        /// </summary>
        /// <param name="configuration">Objeto com as configurações do ambiente</param>
        /// <param name="database">Nome do banco de dados utilizado para buscar as configurações</param>
        public static string GetNpgsqlConnectionString(this IConfiguration configuration, string database)
        {
            string databaseName = configuration.GetValue<string>($"DB_{database}_DATABASE") ??
                            configuration.GetValue<string>($"Database:{database}:DatabaseName") ??
                            throw new System.Exception("Database Schema configuration not found");

            string server = configuration.GetValue<string>($"DB_{database}_SERVER") ??
                            configuration.GetValue<string>($"Database:{database}:Server") ??
                            throw new System.Exception("Database Server configuration not found");

            string port = configuration.GetValue<string>($"DB_{database}_PORT") ??
                          configuration.GetValue<string>($"Database:{database}:Port") ??
                          throw new System.Exception("Database Port configuration not found");

            string user = configuration.GetValue<string>($"DB_{database}_USER") ??
                          configuration.GetValue<string>($"Database:{database}:User") ??
                          throw new System.Exception("Database User configuration not found");

            string password = configuration.GetValue<string>($"DB_{database}_PASSWORD") ??
                              configuration.GetValue<string>($"Database:{database}:Password") ??
                              throw new System.Exception("Database Password configuration not found");

            Npgsql.SslMode sslMode = (Npgsql.SslMode)(configuration.GetValue<int?>($"Database:{database}:SslMode")
                ?? configuration.GetValue<int?>($"DB_{database}_SSLMODE")
                ?? 0);

            bool pooling = configuration.GetValue<bool?>($"Database:{database}:Pooling")
                ?? configuration.GetValue<bool?>($"DB_{database}_POOLING")
                ?? false;

            uint minPooling = configuration.GetValue<uint?>($"Database:{database}:PoolingMin")
                ?? configuration.GetValue<uint?>($"DB_{database}_POOLINGMIN")
                ?? 1;

            uint maxPooling = configuration.GetValue<uint?>($"Database:{database}:PoolingMax")
                ?? configuration.GetValue<uint?>($"DB_{database}_POOLINGMAX")
                ?? 5;

            var connectionString = new Npgsql.NpgsqlConnectionStringBuilder
            {
                Database = databaseName,
                Username = user,
                Password = password,
                Host = server,
                Port = int.Parse(port),
                SslMode = sslMode,
                Pooling = pooling
            };

            if (pooling)
            {
                connectionString.MinPoolSize = (int)minPooling;
                connectionString.MaxPoolSize = (int)maxPooling;
            }
            return connectionString.ToString();
        }

        /// <summary>
        /// Recupera a configuração das variaveis de ambiente
        /// </summary>
        /// <param name="configuration">objeto com as configurações do ambiente</param>
        /// <param name="config">Nome da configuração que está sendo procurada</param>
        /// <param name="required">Se a configuração é obrigatória</param>
        /// <typeparam name="T">Tipo da configuração</typeparam>
        public static T GetConfiguration<T>(this IConfiguration configuration, string config, bool required = true)
        {
            var environmentVariable = configuration[config.ToUpper()];

            if (required && string.IsNullOrWhiteSpace(environmentVariable))
            {
                throw new Exception($"Environment variable '{config.ToUpper()}' don't parametrized");
            }

            return configuration.GetValue<T>(config.ToUpper());
        }

        /// <summary>
        /// Método que adiciona as configurações a injeção de dependencia
        /// </summary>
        /// <param name="services">Services da injeção de dependencia</param>
        /// <param name="configuration">objeto com as configurações do ambiente</param>
        public static void AddConfigurationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("UserOptions");
            services.Configure<UserOptions>(appSettingsSection);
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<UserOptions>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}