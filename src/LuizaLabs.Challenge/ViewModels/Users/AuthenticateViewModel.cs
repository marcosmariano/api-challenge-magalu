using System.ComponentModel.DataAnnotations;

namespace LuizaLabs.Challenge.ViewModels.Users
{
    /// <summary>
    /// View model com os parametros necessários para endpoint de autenticação
    /// </summary>
    public class AuthenticateViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}