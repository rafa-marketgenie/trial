using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using trial.utils;
using trial.Models;
using trial.Services;
using trial.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using trial.Utils;

namespace trial.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<UserSummaryDto>>> GetAll()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetById(Guid id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // [HttpPost]
        // public async Task<ActionResult<UserResponseDto>> Create([FromBody] CreateUserRequestDto request)
        // {
        //     var user = await _userService.CreateUserAsync(request);
        //     if (user == null) return BadRequest("Username or email already exists.");
        //     return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        // }
        
        [HttpPut("{id}")]
        public async Task<ActionResult<UserResponseDto>> Update(Guid id, [FromBody] UpdateUserRequestDto request)
        {
            var actorUserId = User.GetUserId();

            var updatedUser = await _userService.UpdateUserAsync(id, request, actorUserId);
            if (updatedUser == false) return NotFound();
            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var actorUserId = User.GetUserId();

            var success = await _userService.DeleteUserAsync(id, actorUserId);
            
            if (!success) 
            {
                return NotFound();
            }
            
            return NoContent();
        }
    }
}