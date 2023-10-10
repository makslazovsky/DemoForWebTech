using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using TestForWebTech.Models;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;
using TestForWebTechBL.Services;
using TestForWebTechDAL;

namespace TestForWebTech.Controllers
{
    [Route("Users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IWebTechService _webTechService;
        public UserController(IWebTechService webTechService)
        {
            _webTechService = webTechService;
        }
        /// <summary>
        ///  returns filtered users
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /User
        ///     {
        ///        "PropertyOrderName": name,
        ///        "PropertyOrder": "true",
        ///        "SearchText": bars,
        ///        "PropertySearchName": "name",
        ///        "PageNumber": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return filtered users </response>
        /// <response code="400">User already exists or bad input</response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpGet]
        [Authorize(Roles = "Support,Admin, SuperAdmin")]
        public async Task<List<User>> GetAllUsers([FromQuery] Filter filter)
        {
            return await _webTechService.GetAllUsers(filter);
        }

        /// <summary>
        ///  returns user by id
        /// </summary>
        /// <response code="200">Return user by id </response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpGet("{userId}")]
        [Authorize(Roles = "Support,Admin, SuperAdmin")]
        public Task<User> GetUser([FromRoute]int userId)
        {
            return _webTechService.GetUser(userId);
        }

        /// <summary>
        ///  returns roles of user
        /// </summary>
        /// <response code="200">Return roles </response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found roles </response>
        /// <response code="500">Server issue</response>
        [HttpGet("{userId}/Roles")]
        [Authorize(Roles = "Support,Admin, SuperAdmin")]
        public async Task<List<Role>> GetUserRoles([FromRoute] int userId)
        {
            return await _webTechService.GetUserRoles(userId);
        }

        /// <summary>
        ///  ModifyUserRoles
        /// </summary>
        /// <response code="200">User roles are modified </response>
        /// <response code="400">Bad input</response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpPut("{userId}/Roles")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task AssignRoleToUser([FromRoute]int userId, [FromBody]List<int> roleIds)
        {
            await _webTechService.AssignRoleToUser(userId, roleIds);
        }

        /// <summary>
        ///  Create user
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET /User
        ///     {
        ///        "name": Vasya,
        ///        "age": 23,
        ///        "email": vasyan@gmail.com,
        ///        "userRoleIds": 1
        ///     }
        /// </remarks>
        /// <response code="200">User was created </response>
        /// <response code="400">Bad input</response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpPost]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<User> CreateUser([FromBody] UserCreate newUser)
        {
            return await _webTechService.CreateUser(newUser);
        }

        /// <summary>
        ///  Modify user
        /// </summary>
        /// <response code="200">User was modified </response>
        /// <response code="400">Bad input</response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpPatch("{userId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<User> ModifyUser([FromRoute] int userId, [FromBody] UserEdit newUser)
        {
            return await _webTechService.ModifyUser(userId, newUser);
        }

        /// <summary>
        ///  Delete user
        /// </summary>
        /// <response code="200">User was deleted </response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found user </response>
        /// <response code="500">Server issue</response>
        [HttpDelete("{userId}")]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task DeleteUser(int userId)
        {
            await _webTechService.DeleteUser(userId);
        }
    }
}
