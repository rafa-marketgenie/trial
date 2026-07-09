using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using trial.utils;
using trial.Models;
using trial.Services;
using trial.Contracts.Permissions;
using Microsoft.AspNetCore.Authorization;
using trial.Utils;

namespace trial.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class PermissionPolicyController : ControllerBase
    {
        private readonly IPermissionPolicyService _permissionPolicyService;

        public PermissionPolicyController(IPermissionPolicyService permissionPolicyService)
        {
            _permissionPolicyService = permissionPolicyService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermissionPolicyResponseDto>> GetById(int id)
        {
            var actorUserId = User.GetUserId();
            var policy = await _permissionPolicyService.GetPermissionPolicyAsync(id, actorUserId);
            if (policy == null) return NotFound();
            return Ok(policy);
        }

        [HttpPost]
        public async Task<ActionResult<PermissionPolicyResponseDto>> Create([FromBody] GrantPermissionRequestDto request)
        {
            var actorUserId = User.GetUserId();
            var policy = await _permissionPolicyService.GrantPermissionAsync(request, actorUserId);
            return CreatedAtAction(nameof(GetById), new { id = policy.Id }, policy);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePermissionRequestDto request)
        {
            var actorUserId = User.GetUserId();
            var success = await _permissionPolicyService.UpdatePermissionPolicyAsync(id, request, actorUserId);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var actorUserId = User.GetUserId();
            var success = await _permissionPolicyService.DeletePermissionPolicyAsync(id, actorUserId);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}