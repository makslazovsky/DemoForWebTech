using Microsoft.AspNetCore.Mvc;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;

namespace TestForWebTechBL.Services
{
    public interface IWebTechService
    {
        public Task<string> Login(string useremail, string password);
        public Task<List<User>> GetAllUsers([FromQuery] Filter filter);
        public Task<List<Role>> GetAllRoles([FromQuery] Filter filter);
        public Task<User> GetUser(int userId);
        public Task<List<Role>> GetUserRoles(int userId);
        public Task AssignRoleToUser(int userId, List<int> roleIds);
        public Task<User> CreateUser(UserCreate newUser);
        public Task<User> ModifyUser(int userId, UserEdit newUser);
        public Task DeleteUser(int userId);

    }
}

