using Microsoft.AspNetCore.Mvc;
using trial.utils;
using trial.Models;
using trial.Services;
using trial.Contracts.Auth;
using trial.Contracts.Users;

namespace trial.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly TokenService _tokenService;
        private readonly IUserService _userService;

        public AuthController(TokenService tokenService, IUserService userService)
        {
            _tokenService = tokenService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var user = await _userService.AuthenticateUserAsync(request.Username, request.Password);

            if (user == null)
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateUserRequestDto request)
        {
            var user = await _userService.CreateUserAsync(request);

            if (user == null)
                return BadRequest("User registration failed");

            var token = _tokenService.GenerateToken(user);

            return Ok(new { token });
        }
    }
}