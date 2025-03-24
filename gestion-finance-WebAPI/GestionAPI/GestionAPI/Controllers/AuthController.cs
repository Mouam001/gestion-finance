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

        [HttpDelete("delete/{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                bool deleted = await _userService.DeleteUSer(userId);
                if (deleted)
                {
                    return Ok(new { message = "User deleted" });
                }

                return BadRequest(new { message = "User not found" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserRequest request)
        {
            try
            {
                var updateUser = await _userService.UpdateUser(userId, request);
                return Ok(new { message = "User updated", updateUser });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}