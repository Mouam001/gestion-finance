//using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Business.Interfaces;
using Common.Requests;
using Microsoft.AspNetCore.Authorization;

namespace GestionAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthservice _authService;

        /* public AuthController(IUserService userService)
         {
             _userService = userService;
         }*/

        public AuthController(IUserService userService, IAuthservice authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(RegisterRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            
            var user = await _userService.RegisterUser(request);
            return Ok(user);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
           
            var response = await _authService.Login(request);
            return Ok(response);
        }
    
    }
}