using Microsoft.AspNetCore.Mvc;
using TestForWebTechBL.Services;

namespace TestForWebTech.Controllers
{
    [Route("Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IWebTechService _webTechService;
        public AuthController(IWebTechService webTechService)
        {
            _webTechService = webTechService;
        }
        /// <remarks>
        /// Sample request:
        ///
        /// Login as admin
        ///     {
        ///        "email": "tes@gmail.com",
        ///        "password": "123"
        ///     }
        ///
        /// </remarks>
        [HttpPost("login")]
        public Task<string> Login(string userEmail, string password)
        {       
            return _webTechService.Login(userEmail, password);
        }
    }
}
