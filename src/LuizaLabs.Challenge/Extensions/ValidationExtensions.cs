using System;

namespace LuizaLabs.Challenge.Extensions
{
    /// <summary>
    /// Extension para validação de parametros
    /// </summary>
    public static class ValidationExtensions
    {
        /// <summary>
        /// Valida se o parâmetro foi informado
        /// </summary>
        /// <param name="parameter">parâmetro validado</param>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="allowWhiteSpace">Se permite espaços vazios</param>
        /// <exception cref="ArgumentNullException">Ocorre quando o parâmetro esta nulo ou vazio</exception>
        public static void ThrowIfNull(this string parameter, string name, bool allowWhiteSpace = false)
        {
            if ((!allowWhiteSpace && string.IsNullOrWhiteSpace(parameter)) ||
                (allowWhiteSpace && string.IsNullOrEmpty(parameter)))
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Valida se o parâmetro informado não está nulo
        /// </summary>
        /// <param name="parameter">Parâmetro a ser validado</param>
        /// <param name="name">Nome do Parâmetro</param>
        /// <exception cref="ArgumentNullException">Ocorre quando o parâmetro está nulo</exception>
        public static void ThrowIfNull(this object parameter, string name)
        {
            if (parameter == null)
                throw new ArgumentNullException(name);
        }

        /// <summary>
        /// Valida se o valor do parâmetro é menor que o valor minimo permitido
        /// </summary>
        /// <param name="actualValue">Valor validado</param>
        /// <param name="minValue">Valor mínimo permitido</param>
        /// <param name="name">Nome do parâmetro</param>
        /// <exception cref="ArgumentOutOfRangeException">Ocorre quando o valor atual é menor que o mínimo permitido</exception>
        public static void ThrowIfOutOfRange(this int actualValue, int minValue, string name)
        {
            if (actualValue < minValue)
                throw new ArgumentOutOfRangeException(name, actualValue,
                    $"Parameter value must be at least {minValue}");
        }

        /// <summary>
        /// Valida se o valor do parâmetro é menor que o valor minimo permitido
        /// </summary>
        /// <param name="actualValue">Valor validado</param>
        /// <param name="minValue">Valor mínimo permitido</param>
        /// <param name="name">Nome do parâmetro</param>
        /// <exception cref="ArgumentOutOfRangeException">Ocorre quando o valor atual é menor que o mínimo permitido</exception>
        public static void ThrowIfOutOfRange(this long actualValue, int minValue, string name)
        {
            if (actualValue < minValue)
                throw new ArgumentOutOfRangeException(name, actualValue,
                    $"Parameter value must be at least {minValue}");
        }

        /// <summary>
        /// Valida se o parâmetro do tipo Guid é valido
        /// </summary>
        /// <param name="guid">Valor Guid que deve ser validado</param>
        /// <param name="name">Nome do parâmetro</param>
        /// <param name="acceptEmpty">Indica se deve ser considerado o valor Guid.Empty</param>
        /// <exception cref="ArgumentException">Ocorre quando o Guid informado está inválido</exception>
        public static void ThrowIfInvalid(this Guid guid, string name, bool acceptEmpty = false)
        {
            if (Guid.Empty == guid && !acceptEmpty)
                throw new ArgumentException("Parameter Invalid", name);
        }
    }
}