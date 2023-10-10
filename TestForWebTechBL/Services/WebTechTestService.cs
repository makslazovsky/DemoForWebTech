using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;

namespace TestForWebTechBL.Services
{
    public class WebTechTestService : IWebTechService
    {
        private readonly IWebTechStorageService _storageService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public WebTechTestService(IWebTechStorageService storage, IConfiguration configuration, ILogger logger) 
        {
            _storageService = storage;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> Login(string useremail, string password)
        {
            try
            {
                var user = await _storageService.GetUserByEmail(useremail);
                var userRoles = await _storageService.GetUserRoles(user.UserId);

                var claims = userRoles.Select(x => new Claim(ClaimTypes.Role, x.Name)).ToList();
                claims.Add(new Claim("UserId", user.UserId.ToString()));

                var secret = _configuration["JwtSettings:Secret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

                var token = new JwtSecurityToken(
                  claims: claims,
                  notBefore: DateTime.Now,
                  expires: DateTime.Now.AddHours(12),
                  signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to login", ex);
                throw;
            }
        }

        public async Task AssignRoleToUser(int userId, List<int> roleIds)
        {
            try
            {
                await CheckUserExists(userId);
                await CheckRolePresent(roleIds);
                await _storageService.AssignRoleToUser(userId, roleIds);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to assign role to User", ex);
                throw;
            }
        }

        public async Task<User> CreateUser(UserCreate newUser)
        {
            try
            {
                _logger.Information("Creating new user");
                await ValidateCreateModel(newUser);
                _logger.Information("New user created");
                return await _storageService.CreateUser(newUser);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to create User", ex);
                throw;
            }
            
        }

        public Task<List<User>> GetAllUsers([FromQuery]Filter filter)
        {
            try
            {
                return _storageService.GetAllUsers(filter);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get All users", ex);
                throw;
            }
            
        }

        public Task<List<Role>> GetAllRoles([FromQuery] Filter filter)
        {
            try
            {
                return _storageService.GetAllRoles(filter);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get all roles", ex);
                throw;
            }
           
        }

        public Task<User> GetUser(int userId)
        {
            try
            {
                return _storageService.GetUser(userId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get User by id {userId}", ex);
                throw;
            }
            
        }

        public Task<List<Role>> GetUserRoles(int userId)
        {
            try
            {
                return _storageService.GetUserRoles(userId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get UserRoles {userId}", ex);
                throw;
            }
           
        }

        public async Task<User> ModifyUser(int userId, UserEdit newUser)
        {
            try
            {
                _logger.Information($"Modifying user {userId}");
                await ValidateEditModel(newUser);
                await CheckEmail(newUser.Email);
                return await _storageService.ModifyUser(userId, newUser);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to modify user {userId}", ex);
                throw;
            }
        }

        public async Task DeleteUser(int userId)
        {
            try
            {
                _logger.Information($"Deleting user {userId}");
                await CheckUserExists(userId);
                await _storageService.DeleteUser(userId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to delete user {userId}", ex);
                throw;
            }
           
        }

        private async Task ValidateCreateModel(UserCreate user)
        {
            await CheckEmail(user.Email);
            CheckAge(user.Age);
            CheckName(user.Name);
            await CheckRoles(user.UserRoleIds);
        }

        private async Task CheckRoles(List<int> userRoleIds)
        {
            var roles = await _storageService.GetRoles(userRoleIds);
            if (roles.Count != userRoleIds.Count || userRoleIds.Count == 0)
            {
                throw new BaseException(ErrorCodes.Unknown);
            }
        }

        private async Task ValidateEditModel(UserEdit user)
        {
            if (user.Name != null)
                CheckName(user.Name);
            if (user.Email != null)
                await CheckEmail(user.Email);
            if (user.Age != null)
                CheckAge(user.Age.Value);
        }

        private void CheckAge(int age)
        {
            if (age < 0)
                throw new BaseException(ErrorCodes.Unknown);
        }

        private void CheckName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new BaseException(ErrorCodes.Unknown);
        }

        private async Task CheckUserExists(int userId)
        {
            var user = await _storageService.GetUser(userId);
            if (user == null)
            {
                _logger.Warning($"User not found: Id {userId}");
                throw new BaseException(ErrorCodes.NotFound);
            }
        }

        private async Task CheckRolePresent(List<int> userRoleIds)
        {
            var userRoles = await _storageService.GetRoles(userRoleIds);
            if (userRoles.Count != userRoleIds.Count)
            {
                throw new BaseException(ErrorCodes.NotFound);
            }
        }

        private async Task CheckEmail(string email)
        {
            try
            {
                new MailAddress(email);
            }
            catch (FormatException)
            {
                throw new BaseException(ErrorCodes.BadUserInput);
            }
            var user = await _storageService.GetUserByEmail(email);
            if (user != null)
            {

                throw new BaseException(ErrorCodes.AlreadyExists);
            }
        }
    }
}
