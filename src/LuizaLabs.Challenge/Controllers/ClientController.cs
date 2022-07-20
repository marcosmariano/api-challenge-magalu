using System;
using System.Net;
using System.Threading.Tasks;
using LuizaLabs.Challenge.Business;
using LuizaLabs.Challenge.Models;
using LuizaLabs.Challenge.ViewModels.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace LuizaLabs.Challenge.Controllers
{
    /// <summary>
    /// Controller responsavel por dados de cliente
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/v1/clients")]
    public class ClientController : ControllerBase
    {
        private IClientBusiness _clientBusiness;

        /// <summary>
        /// Construtor da controller
        /// </summary>
        /// <param name="userService">Service de usuário</param>
        public ClientController(IClientBusiness clientBusiness)
        {
            _clientBusiness = clientBusiness ?? throw new ArgumentNullException(nameof(clientBusiness));
        }

        /// <summary>
        /// Método que faz a busca de todos clientes
        /// </summary>
        [Authorize(Roles = Role.AdminAndUser)]
        [HttpGet(Name = "GetAllClients")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAllClientsAsync([FromQuery] int pageNumber = 0, [FromQuery] int pageSize = 100)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var clients = await _clientBusiness.GetAllAsync(pageNumber, pageSize);
            return Ok(clients);
        }

        /// <summary>
        /// Método que faz busca de cliente por Id
        /// </summary>
        /// <param name="id">Id do cliente</param>
        [Authorize(Roles = Role.AdminAndUser)]
        [HttpGet("{id}", Name = "GetClientById")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetClientByIdAsync([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = await _clientBusiness.GetByIdAsync(id);

            if (client == null)
                return NotFound();

            return Ok(client);
        }

        /// <summary>
        /// Método que faz delete de cliente por id
        /// </summary>
        /// <param name="id">Id do cliente</param>
        [Authorize(Roles = Role.AdminAndUser)]
        [HttpDelete("{id}", Name = "DeleteClientById")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> DeleteClientById([FromRoute] int id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (await _clientBusiness.GetByIdAsync(id) != null)
            {
                var client = await _clientBusiness.DeleteAsync(id);
                return Ok("Client deleted");
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Método que faz update de cliente por id
        /// </summary>
        /// <param name="id">Id do cliente</param>
        [Authorize(Roles = Role.AdminAndUser)]
        [HttpPut("{id}", Name = "UpdateClientById")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateClientById([FromRoute] int id, [FromBody] UpdateAndCreateViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = await _clientBusiness.GetByIdAsync(id);

            if (client != null)
            {
                client.Email = request.Email;
                client.Name = request.Name;
                await _clientBusiness.UpdateAsync(client);
                return Ok(client);
            }
            else
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Método para criar novo cliente
        /// </summary>
        /// <param name="request">Dados de cliente</param>
        [Authorize(Roles = Role.AdminAndUser)]
        [HttpPost(Name = "CreateClientAsync")]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CreateClientAsync([FromBody] UpdateAndCreateViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var client = new Client()
            {
                Email = request.Email,
                Name = request.Name
            };

            try
            {
                client = await _clientBusiness.CreateAsync(client);
            }
            catch (DbUpdateException dbx) when (dbx.InnerException is PostgresException &&
                                                dbx.InnerException.Message.Contains("23505: duplicate key value violates unique constraint"))
            {
                return StatusCode((int)HttpStatusCode.Conflict, "Client already exists");
            }
            return StatusCode((int)HttpStatusCode.Created, client);
        }
    }
}