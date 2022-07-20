using System;
using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Models;
using Microsoft.Extensions.Logging;

namespace LuizaLabs.Challenge.Business
{
    /// <summary>
    /// Classe base de negócios
    /// </summary>
    /// <typeparam name="TEntity">Tipo da entidade que será manipulada pela classe de negócios</typeparam>
    public abstract class BaseBusiness<TEntity> : IDisposable where TEntity : class, IModelBase
    {
        private bool _disposed;
        protected ILogger<BaseBusiness<TEntity>> Logger;
        protected IUnitOfWork UnitOfWork;

        protected BaseBusiness(ILogger<BaseBusiness<TEntity>> logger, IUnitOfWork unitOfWork)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        /// <summary>
        /// Cria uma exceção quando a classe já teve o métido Dispose utilizado
        /// </summary>
        protected virtual void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Retorna o repositório da entidade
        /// </summary>
        /// <typeparam name="TEntity">Tipo da entidade do repositório</typeparam>
        protected virtual IRepository<TEntity> GetRepository() => UnitOfWork.GetRepository<TEntity>();

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

            if (disposing)
            {
                UnitOfWork.Dispose();
                UnitOfWork = null;
            }

            Logger = null;

            _disposed = true;
        }
    }
}