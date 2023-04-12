using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tracker.Application.Commands.EmployeeCommands.CreateEmployeeCommand;
using Tracker.Application.Commands.EmployeeCommands.DeleteEmployeeCommand;
using Tracker.Application.Commands.EmployeeCommands.UpdateEmployeeCommand;
using Tracker.Application.Queries.EmployeeQueries.GetAllEmployeesQuery;
using Tracker.Application.Queries.EmployeeQueries.GetEmployeeByIdQuery;
using Tracker.WebApi.Infrastructure.Requests;

namespace Tracker.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMediator _mediator;

        public EmployeeController(ILogger<EmployeeController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async ValueTask<IActionResult> GetAllEmployeesAsync()
        {
            var result = await _mediator.Send(new GetAllEmployeesQuery());

            if (result is null)
            {
                _logger.LogInformation("No employees found");
                return NoContent();
            }

            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        public async ValueTask<IActionResult> GetSingleEmployeeAsync(Guid id)
        {
            var result = await _mediator.Send(new GetEmployeeByIdQuery(id));

            if (result is null)
            {
                _logger.LogInformation("No employees found");
                return NoContent();
            }

            return Ok(result);
        }


        // [HttpPost]
        // public async ValueTask<IActionResult> CreateProjectAsync([FromBody] CreateProjectRequest data)
        // {
        //     var command = data.Adapt<CreateProjectCommand>();

        //     Guid result = await _mediator.Send(command);

        //     return result != Guid.Empty ? Ok(result) : BadRequest();
        // }

        [HttpPost]
        public async ValueTask<IActionResult> CreateEmployeeAsync([FromBody] CreateEmployeeRequest request)
        {
            var mappedRequest = request.Adapt<CreateEmployeeCommand>();
            var result = await _mediator.Send(mappedRequest);

            if (result == Guid.Empty)
            {
                _logger.LogInformation("Can not create employee");
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpPut]
        public async ValueTask<IActionResult> UpdateEmployeeAsync([FromBody] UpdateEmployeeRequest request)
        {
            var mappedRequest = request.Adapt<UpdateEmployeeCommand>();
            var result = await _mediator.Send(mappedRequest);

            if (result == Guid.Empty)
            {
                _logger.LogInformation("Can not update employee");
                return BadRequest();
            }

            return Ok(result);
        }

        [HttpDelete]
        [Route("{id}")]
        public async ValueTask<IActionResult> DeleteEmployeeAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteEmployeeCommand(id));

            if (result == Guid.Empty)
            {
                _logger.LogInformation("Can not delete employee");
                return BadRequest();
            }

            return Ok(result);
        }
    }
}