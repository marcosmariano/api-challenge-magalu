using System.Collections.Generic;
using System.Linq;
using LuizaLabs.Challenge.Models;

namespace LuizaLabs.Challenge.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// Retira a senha de usuários
        /// </summary>
        /// <param name="users">Lista de usuários a ser retirado</param>
        /// <returns>Lista de usuários sem senha</returns>
        public static IEnumerable<User> WithoutPasswords(this IEnumerable<User> users)
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        /// <summary>
        /// Retira a senha de usuário
        /// </summary>
        /// <param name="user">Usuário a ser retirado senha</param>
        /// <returns>Usuário sem a senha</returns>
        public static User WithoutPassword(this User user)
        {
            if (user == null) return null;

            user.Password = null;
            return user;
        }
    }
}