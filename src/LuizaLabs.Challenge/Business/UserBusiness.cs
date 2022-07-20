using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Arch.EntityFrameworkCore.UnitOfWork;
using LuizaLabs.Challenge.Extensions;
using LuizaLabs.Challenge.Infra.Configurations.Options;
using LuizaLabs.Challenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LuizaLabs.Challenge.Business
{
    /// <summary>
    /// Business de autenticação de usuário
    /// </summary>
    public interface IUserBusiness
    {
        /// <summary>
        /// Método responsavel por autorizar usuário
        /// </summary>
        /// <param name="username">Nome usuário</param>
        /// <param name="password">Senha usuário</param>
        /// <returns>Retorna usuário</returns>
        Task<User> AuthenticateAsync(string username, string password);

        /// <summary>
        /// Método responsável por buscar usuários
        /// </summary>
        /// <returns>Retorna lista de usuários</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Método responsável por buscar usuário por Id
        /// </summary>
        /// <param name="id">Id de usuário</param>
        /// <returns>Retorna usuário</returns>
        Task<User> GetByIdAsync(int id);

        /// <summary>
        /// Método responsável por buscar usuário por username e password
        /// </summary>
        /// <param name="username">Nome de usuário</param>
        /// <param name="password">Senha de usuário</param>
        /// <returns>Retorna usuário</returns>
        Task<User> GetByUsernameAndPasswordAsync(string username, string password);

        /// <summary>
        /// Método responsável por criar usuário
        /// </summary>
        /// <param name="user">Objeto com dados de usuário</param>
        /// <returns>Retorna usuário</returns>
        Task<User> CreateAsync(User user);
    }
    public class UserBusiness : BaseBusiness<User>, IUserBusiness
    {
        private readonly UserOptions _userOptions;

        /// <summary>
        /// Construtor da business
        /// </summary>
        /// <param name="logger">Logger da classe</param>
        /// <param name="userOptions">Opções de usuário</param>
        /// <returns></returns>
        public UserBusiness(ILogger<UserBusiness> logger, IUnitOfWork unitOfWork,
                            IOptions<UserOptions> userOptions) : base(logger, unitOfWork)
        {
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userOptions = userOptions.Value ?? throw new ArgumentNullException(nameof(userOptions));
        }

        /// <inheritdoc cref="IUserBusiness.Authenticate"/>
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            username.ThrowIfNull(nameof(username));
            password.ThrowIfNull(nameof(password));

            Logger.LogInformation($"Autenticando usuário - username: {username}");
            var user = await GetByUsernameAndPasswordAsync(username, password);

            if (user == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_userOptions.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            Logger.LogInformation($"Autenticação - user: {username} - Autorizado");
            return user;
        }

        /// <inheritdoc cref="IUserBusiness.GetAllAsync"/>
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            Logger.LogDebug($"Buscando Lista de usuários");
            IRepository<User> repository = GetRepository();
            IEnumerable<User> users = null;
            try
            {
                var pagedList = await repository.GetPagedListAsync(orderBy: x => x.OrderBy(entity => entity.Id));
                users = pagedList?.Items;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao buscar lista de usuários - GetAllAsync. {ex.Message}");
                throw;
            }
            return users.WithoutPasswords();
        }

        /// <inheritdoc cref="IUserBusiness.GetByIdAsync"/>
        public async Task<User> GetByIdAsync(int id)
        {
            ThrowIfDisposed();
            id.ThrowIfOutOfRange(0, nameof(id));

            Logger.LogInformation($"Buscando usuário - GetByIdAsync - id: {id}");
            IRepository<User> repository = GetRepository();
            User user = null;
            try
            {
                user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Id == id);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao buscar informações de usuário - GetByIdAsync - id: {id}. {ex.Message}");
                throw;
            }

            return user.WithoutPassword();
        }

        /// <inheritdoc cref="IUserBusiness.GetByUsernameAndPasswordAsync"/>
        public async Task<User> GetByUsernameAndPasswordAsync(string username, string password)
        {
            ThrowIfDisposed();
            username.ThrowIfNull(nameof(username));
            password.ThrowIfNull(nameof(password));

            Logger.LogInformation($"Buscando usuário - GetByUsernameAndPasswordAsync - username: {username}");
            IRepository<User> repository = GetRepository();
            User user = null;
            try
            {
                user = await repository.GetFirstOrDefaultAsync(predicate: x => x.Username == username && x.Password == password);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao buscar informações de usuário - GetByUsernameAndPasswordAsync - username: {username}. {ex.Message}");
                throw;
            }

            return user.WithoutPassword();
        }

        /// <inheritdoc cref="IUserBusiness.CreateAsync"/>
        public async Task<User> CreateAsync(User user)
        {
            ThrowIfDisposed();
            user.ThrowIfNull(nameof(user));

            IRepository<User> repository = GetRepository();
            Logger.LogInformation($"Inserindo usuário - CreateAsync - username: {user.Username}");
            try
            {
                await repository.InsertAsync(user);
                await UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Erro ao criar usuário - CreateAsync - username:'{user.Username}'. {ex.Message}");
                throw;
            }

            return user.WithoutPassword();
        }
    }
}