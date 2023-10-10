using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;
using System.Linq;
using TestForWebTechBL.Services;

namespace TestForWebTechDAL.Services
{
    public class WebTechStorageService : IWebTechStorageService
    {
        private readonly RepositoryContext _context;
        private readonly string userNameProperty = nameof(User.Name).ToLower();
        private readonly string userAgeProperty = nameof(User.Age).ToLower();
        private readonly string userEmailProperty = nameof(User.Email).ToLower();
        private readonly string roleNameProperty = nameof(Role.Name).ToLower();
        const int pageSize = 10;
        public WebTechStorageService(RepositoryContext context)
        {
            _context = context;
        }

        public async Task AssignRoleToUser(int userId, List<int> roleIds)
        {
           var result = await _context.UserRoles.Where(x => x.UserId == userId).ToListAsync();
           var rolesToDelete = result.Where(x => !roleIds.Contains(x.RoleId)).ToList();
           var rolesToAdd = roleIds.Where(x => result.All(y=>y.RoleId!=x)).ToList();

           _context.UserRoles.RemoveRange(rolesToDelete);
           _context.UserRoles.AddRange(rolesToAdd.Select(x=>new UserRole {UserId = userId, RoleId = x }));
           await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User> CreateUser(UserCreate newUser)
        {
            User user = new User
            {
                Name = newUser.Name,
                Age = newUser.Age,
                Email = newUser.Email
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            await AssignRoleToUser(user.UserId, newUser.UserRoleIds);
            return user;
        }

        public Task<List<User>> GetAllUsers(Filter filter)
        {
            IQueryable<User> query = _context.Users;
            if (filter.PropertySearchName == userNameProperty)
            {
                query = query.Where(x => x.Name.Contains(filter.SearchText));
            }
            else if (filter.PropertySearchName == userAgeProperty)
            {
                query = query.Where(x => x.Age == int.Parse(filter.SearchText));
            }
            else if (filter.PropertySearchName == userEmailProperty)
            {
                query = query.Where(x => x.Email.Contains(filter.SearchText));
            }


            if (filter.PropertyOrderName == userNameProperty)
            {
                query = filter.PropertyOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
            else if (filter.PropertyOrderName == userAgeProperty)
            {
                query = filter.PropertyOrder ? query.OrderBy(x => x.Age) : query.OrderByDescending(x => x.Age);
            }
            else if (filter.PropertyOrderName == userEmailProperty)
            {
                query = filter.PropertyOrder ? query.OrderBy(x => x.Email) : query.OrderByDescending(x => x.Email);
            }

            query = query.Skip(filter.PageNumber * pageSize).Take(pageSize);
            return query.ToListAsync();
        }

        public Task<List<Role>> GetAllRoles(Filter filter)
        {
            IQueryable<Role> query = _context.Roles;
            query = query.Where(x => x.Name.Contains(filter.SearchText));
            query = filter.PropertyOrder ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);

            query = query.Skip(filter.PageNumber * pageSize).Take(pageSize);
            return query.ToListAsync();
        }

        public async Task<List<Role>> GetUserRoles(int userId)
        {
            return await _context.UserRoles.Where(x => x.UserId == userId).Select(x=>x.Role).ToListAsync();
        }

        public async Task<User> ModifyUser(int userId, UserEdit patchUser)
        {
                User modifiedUser = _context.Users.FirstOrDefault(x => x.UserId == userId);
                if (patchUser.Name != null) 
                {
                    modifiedUser.Name = patchUser.Name;
                }
                if (patchUser.Email != null)
                {
                    modifiedUser.Email = patchUser.Email;
                }
                if (patchUser.Age != null)
                {
                    modifiedUser.Age = patchUser.Age.Value;
                }
            
                await _context.SaveChangesAsync();
                return modifiedUser;
        }

        public async Task DeleteUser(int userId)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            _context.Users.Remove(existingUser);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserByEmail(string email) 
        {
            return await _context.Users.FirstOrDefaultAsync(x=>x.Email == email);
        }

        public async Task<List<Role>> GetRoles(IEnumerable<int> userRoleIds)
        {
            return await _context.Roles.Where(x => userRoleIds.Contains(x.RoleId)).ToListAsync();
        }
    }
}
