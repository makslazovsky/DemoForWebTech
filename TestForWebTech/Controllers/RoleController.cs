using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestForWebTechBl.Models;
using TestForWebTechBL.Models;
using TestForWebTechBL.Services;

namespace TestForWebTech.Controllers
{
    [Route("Role")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IWebTechService _webTechService;
        public RoleController(IWebTechService webTechService)
        {
            _webTechService = webTechService;
        }
        /// <summary>
        ///  returns filtered roles
        /// </summary>
        ///  <remarks>
        /// Sample request:
        ///
        ///     GET /Role
        ///     {
        ///        "PropertyOrderName": name,
        ///        "PropertyOrder": "true",
        ///        "SearchText": admin,
        ///        "PropertySearchName": "name",
        ///        "PageNumber": 0
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Return filtered roles </response>
        /// <response code="400">Role bad input</response>
        /// <response code="403">Forbidden </response>
        /// <response code="404">Not found role </response>
        /// <response code="500">Server issue</response>
        [HttpGet]
        [Authorize(Roles = "Admin, SuperAdmin")]
        public async Task<List<Role>> GetAllRoles([FromQuery] Filter filter)
        {
            return await _webTechService.GetAllRoles(filter);
        }
    }
}
