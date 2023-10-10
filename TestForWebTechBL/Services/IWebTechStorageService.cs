using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;

namespace TestForWebTechBL.Services
{
    public interface IWebTechStorageService
    {
        public Task<List<User>> GetAllUsers(Filter filter);
        public Task<List<Role>> GetAllRoles(Filter filter);
        public Task<List<Role>> GetRoles(IEnumerable<int> userRoleIds);
        public Task<User> GetUserByEmail(string email);
        public Task<User> GetUser(int userId);
        public Task<List<Role>> GetUserRoles(int userId);
        public Task AssignRoleToUser(int userId, List<int> roleIds);
        public Task<User> CreateUser(UserCreate newUser);
        public Task<User> ModifyUser(int userId, UserEdit newUser);
        public Task DeleteUser(int userId);
    }
}
