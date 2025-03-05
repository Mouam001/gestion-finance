//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Common.Requests;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]

        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var user = await _userService.RegisterUser(request);
            return Ok(user);
        }
    }
}