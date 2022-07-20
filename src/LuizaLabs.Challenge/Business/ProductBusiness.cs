using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Extensions;
using LuizaLabs.Challenge.Models;
using LuizaLabs.Challenge.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LuizaLabs.Challenge.Business
{
    /// <summary>
    /// Business para manipulação de produtos
    /// </summary>
    public interface IProductBusiness : IDisposable
    {
        /// <summary>
        /// Método para busca de produto por Id
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Objeto de Produto</returns>
        Task<Product> GetByIdAsync(Guid id);

        /// <summary>
        /// Método para busca de produtos
        /// </summary>
        /// <param name="pageNumber">Número da pagina</param>
        /// <returns>Lista de Produtos</returns>
        Task<List<Product>> GetAllAsync(int pageNumber = 1);
    }

    public class ProductBusiness : IProductBusiness
    {
        private bool _disposed;
        private ILogger<ProductBusiness> _logger;
        private readonly IProductApiService _productApiService;
        public ProductBusiness(ILogger<ProductBusiness> logger,
                            IProductApiService productApiService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _productApiService = productApiService ?? throw new ArgumentNullException(nameof(productApiService));
        }

        /// <inheritdoc cref="IProductBusiness.GetAllAsync"/>
        public async Task<List<Product>> GetAllAsync(int pageNumber = 1)
        {
            ThrowIfDisposed();
            HttpResponseMessage responseMessage = null;

            responseMessage = await _productApiService.GetAllAsync();
            var response = JsonConvert.DeserializeObject<ProductResponse>(await responseMessage.Content.ReadAsStringAsync());
            return response?.Products;
        }

        /// <inheritdoc cref="IProductBusiness.GetByIdAsync"/>
        public async Task<Product> GetByIdAsync(Guid id)
        {
            ThrowIfDisposed();
            id.ThrowIfInvalid(nameof(id));

            var responseMessage = await _productApiService.GetByIdAsync(id);
            return JsonConvert.DeserializeObject<Product>(await responseMessage.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Cria uma exceção quando a classe já teve o métido Dispose utilizado
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Libera recursos utilizados pela classe
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _logger = null;
            _productApiService.Dispose();

            _disposed = true;
        }
    }
}