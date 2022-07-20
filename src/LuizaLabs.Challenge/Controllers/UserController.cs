using System;
using System.Net;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Models;
using LuizaLabs.Challenge.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LuizaLabs.Challenge.Controllers
{
    /// <summary>
    /// Controller responsavel por autenticação de usuário
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/users")]
    public class UserController : ControllerBase
    {
        private IUserBusiness _userBusiness;

        /// <summary>
        /// Construtor da controller
        /// </summary>
        /// <param name="userService">Service de usuário</param>
        public UserController(IUserBusiness userBusiness)
        {
            _userBusiness = userBusiness ?? throw new ArgumentNullException(nameof(userBusiness));
        }

        /// <summary>
        /// Método responsável por autenticar
        /// </summary>
        /// <param name="model">View model de autenticação</param>
        [AllowAnonymous]
        [HttpPost("authenticate", Name = "AuthenticateAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticateViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _userBusiness.AuthenticateAsync(model.Username, model.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        /// <summary>
        /// Método que faz a busca de todos usuários
        /// Contendo regra de acesso penas por usuários admins
        /// </summary>
        [Authorize(Roles = Role.Admin)]
        [HttpGet(Name = "GetAllUsersAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var users = await _userBusiness.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Método que faz busca de usuário por Id
        /// Permite apenas Admin's ou se for o id do usuário corrente autenticado
        /// </summary>
        /// <param name="id">Id do usuário</param>
        [HttpGet("{id}", Name = "GetUserByIdAsync")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetUserByIdAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Role.Admin))
                return Forbid();

            var user = await _userBusiness.GetByIdAsync(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Método para criar novo usuário
        /// </summary>
        /// <param name="request">Dados de usuário</param>
        [Authorize(Roles = Role.Admin)]
        [HttpPost(Name = "CreateUserAsync")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateUserAsync([FromBody] CreateViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Password = request.Password,
                Role = request.Role,
                Username = request.Username
            };
            return StatusCode((int)HttpStatusCode.Created, await _userBusiness.CreateAsync(user));
        }
    }
}