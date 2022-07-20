using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace LuizaLabs.Challenge.Extensions
{
    /// <summary>
    /// Extensões para adicionar a documentação de APIs ao projeto
    /// </summary>
    public static class SwagggerExtensions
    {
        /// <summary>
        /// Adiciona a documentação da API ao container de injeção de dependência
        /// </summary>
        /// <param name="services">Coleção com os objetos que serão gerenciados pelo container de injeção de dependência</param>
        public static void AddAppDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LuizaLabs.Challenge", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme()
                    {
                        In = ParameterLocation.Header,
                        Description = "Entre com o token de autenticação",
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                c.DescribeAllParametersInCamelCase();
                c.CustomSchemaIds(x => x.FullName);
                c.OrderActionsBy(x => x.RelativePath);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new string[] { }
                    }
                });
            });
        }

        /// <summary>
        /// Utiliza o middleware para expor a documentação das APIs
        /// </summary>
        /// <param name="app">Builder de configuração da aplicação</param>
        public static void UseAppDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LuizaLabs.Challenge v1"));
        }
    }
}