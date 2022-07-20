using System;

namespace LuizaLabs.Challenge.Models
{
    /// <summary>
    /// Contrato base para entidades
    /// </summary>
    public interface IModelBase
    {
        /// <summary>
        /// Código da entidade
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Data e hora da criação do registro
        /// </summary>
        DateTimeOffset CreatedAt { get; set; }

        /// <summary>
        /// Data e hora da última atualização do registro
        /// </summary>
        DateTimeOffset UpdatedAt { get; set; }
    }

    public class ModelBase : IModelBase
    {
        /// <summary>
        /// Código da entidade
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Data e hora da criação do registro
        /// </summary>
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        /// <summary>
        /// Data e hora da última atualização do registro
        /// </summary>
        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}