using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Extensions;
using LuizaLabs.Challenge.Models;
using Microsoft.Extensions.Logging;

namespace LuizaLabs.Challenge.Business
{
    /// <summary>
    /// Business responsável por manipular dados de cliente
    /// </summary>
    public interface IClientBusiness
    {
        /// <summary>
        /// Método responsável pela busca de cliente por id na base
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Cliente</returns>
        Task<Client> GetByIdAsync(int id);

        /// <summary>
        /// Método responsável pela busca de todos clientes na base
        /// </summary>
        /// <param name="pageNumber">Número da pagina (Parametro opcional)</param>
        /// <returns>Lista de Clientes</returns>
        Task<IEnumerable<Client>> GetAllAsync(int pageNumber = 0, int pageSize = 100);

        /// <summary>
        /// Método responsável por atualizar cliente na base
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Cliente</returns>
        Task<Client> UpdateAsync(Client client);

        /// <summary>
        /// Método responsável por inserir cliente na base
        /// </summary>
        /// <param name="client"></param>
        /// <returns>Cliente</returns>
        Task<Client> CreateAsync(Client client);

        /// <summary>
        /// Método responsável por deletar cliente na base
        /// </summary>
        /// <param name="id">Id do cliente</param>
        /// <returns>Boolean indicando se cliente foi removido da base</returns>
        Task<bool> DeleteAsync(int id);
    }
    public class ClientBusiness : BaseBusiness<Client>, IClientBusiness
    {
        public ClientBusiness(ILogger<ClientBusiness> logger, IUnitOfWork unitOfWork) : base(logger, unitOfWork)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <inheritdoc cref="IClientBusiness.DeleteAsync"/>
        public async Task<bool> DeleteAsync(int id)
        {
            ThrowIfDisposed();
            id.ThrowIfOutOfRange(0, nameof(id));
            IRepository<Client> repository = GetRepository();
            try
            {
                repository.Delete(id);
                await UnitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro deletar cliente - DeleteAsync - id: {id}. {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc cref="IClientBusiness.GetAllAsync"/>
        public async Task<IEnumerable<Client>> GetAllAsync(int pageNumber = 0, int pageSize = 100)
        {
            ThrowIfDisposed();
            IRepository<Client> repository = GetRepository();
            try
            {
                var pagedClients = await repository.GetPagedListAsync(pageIndex: pageNumber, pageSize: pageSize);
                return pagedClients?.Items;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao buscar clientes - GetAllAsync. {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc cref="IClientBusiness.GetByIdAsync"/>
        public async Task<Client> GetByIdAsync(int id)
        {
            ThrowIfDisposed();
            id.ThrowIfOutOfRange(0, nameof(id));
            IRepository<Client> repository = GetRepository();
            try
            {
                return await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao buscar cliente - GetByIdAsync - id: {id}. {ex.Message}");
                throw;
            }
        }

        /// <inheritdoc cref="IClientBusiness.CreateAsync"/>
        public async Task<Client> CreateAsync(Client client)
        {
            ThrowIfDisposed();
            client.ThrowIfNull(nameof(client));
            IRepository<Client> repository = GetRepository();
            try
            {
                await repository.InsertAsync(client);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, $"Erro ao criar cliente - CreateAsync - email:'{client.Email}'. {ex.Message}");
                throw;
            }

            return client;
        }

        /// <inheritdoc cref="IClientBusiness.UpdateAsync"/>
        public async Task<Client> UpdateAsync(Client client)
        {
            ThrowIfDisposed();
            client.ThrowIfNull(nameof(client));
            IRepository<Client> repository = GetRepository();
            try
            {
                repository.Update(client);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (System.Exception ex)
            {
                Logger.LogError(ex, $"Erro ao atualizar cliente - UpdateAsync - email:'{client.Email}'. {ex.Message}");
                throw;
            }

            return client;
        }
    }
}