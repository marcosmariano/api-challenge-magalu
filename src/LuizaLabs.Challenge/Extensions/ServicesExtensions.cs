using System;
using System.Net.Http;
using LuizaLabs.Challenge.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestEase;

namespace LuizaLabs.Challenge.Extensions
{
    public static class ServicesExtensions
    {
        public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IProductApiService>(provider =>
            {
                var client = new HttpClient()
                {
                    BaseAddress = new System.Uri(configuration.GetConfiguration<string>("PRODUCT_API_URL") ??
                                                 configuration.GetValue<string>($"Api:Product:Url")),
                    Timeout = TimeSpan.FromSeconds(configuration.GetConfiguration<int?>("PRODUCT_API_TIMEOUT", false) ??
                                                   configuration.GetValue<int?>($"Api:Product:Timeout") ?? 30)
                };
                var api = RestClient.For<IProductApiService>(client);

                return api;
            });
        }
    }
}