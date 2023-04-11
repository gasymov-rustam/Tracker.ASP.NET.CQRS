using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tracker.Application.Commands.RoleCommands.CreateRoleCommand;
using Tracker.Application.Commands.RoleCommands.DeleteRoleCommand;
using Tracker.Application.Commands.RoleCommands.UpdateRoleCommand;
using Tracker.Application.Queries.RoleQueries.GetAllRoles;
using Tracker.Application.Queries.RoleQueries.GetRoleById;
using Tracker.WebApi.Infrastructure.Requests;

namespace Tracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
    public class RolesController : Controller
    {
        private readonly ILogger<RolesController> _logger;
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator, ILogger<RolesController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllRolesAsync()
        {
            var result = await _mediator.Send(new GetAllRolesQuery());

            if (result is null)
            {
                _logger.LogInformation("No roles found");
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async ValueTask<IActionResult> GetRoleByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new GetRoleByIdQuery(id));

            if (result == Guid.Empty)
            {
                _logger.LogInformation("No roles found");
                return NoContent();
            }

            return Ok(result);
        }

        [HttpPost]
        public async ValueTask<IActionResult> CreateRoleAsync([FromBody] string name)
        {
            Guid result = await _mediator.Send(new CreateRoleCommand(name));

            if (result == Guid.Empty)
            {
                _logger.LogError("Role does not created");
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPut]
        public async ValueTask<IActionResult> UpdateRoleAsync([FromBody] UpdateRoleRequest data)
        {
            var command = data.Adapt<UpdateRoleCommand>();

            Guid result = await _mediator.Send(command);

            if (result == Guid.Empty)
            {
                _logger.LogError("Role does not created");
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async ValueTask<IActionResult> DeleteRoleAsync(Guid id)
        {
            Guid result = await _mediator.Send(new DeleteRoleCommand(id));

            if (result == Guid.Empty)
            {
                _logger.LogError("Role does not exist");
                return BadRequest();
            }

            return Ok(result);
        }
    }
}