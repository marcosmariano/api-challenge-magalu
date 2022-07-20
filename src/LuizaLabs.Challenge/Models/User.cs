namespace LuizaLabs.Challenge.Models
{
    /// <summary>
    /// Classe de Model para Usuarios de autorização da Api
    /// </summary>
    public class User : ModelBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}