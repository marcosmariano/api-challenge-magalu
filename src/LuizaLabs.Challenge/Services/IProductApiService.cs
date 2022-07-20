using System;
using System.Net.Http;
using System.Threading.Tasks;
using RestEase;

namespace LuizaLabs.Challenge.Services
{
    /// <summary>
    /// Interface Service para Api de Produtos
    /// </summary>
    public interface IProductApiService : IDisposable
    {
        /// <summary>
        /// Busca Produto por id
        /// </summary>
        /// <param name="id">Id do produto</param>
        /// <returns>Retorna o Objeto da Classe HttpResponseMessage </returns>
        [Get("/api/product/{id}/")]
        Task<HttpResponseMessage> GetByIdAsync([Path("id")] Guid id);

        /// <summary>
        /// Busca Produtos
        /// </summary>
        /// <param name="pageNumber">Numero da pagina com valor default de 1</param>
        /// <returns>Retorna o Objeto da Classe HttpResponseMessage </returns>
        [Get("/api/product/?page={pageNumber}")]
        Task<HttpResponseMessage> GetAllAsync([Path("pageNumber")] int pageNumber = 1);
    }
}