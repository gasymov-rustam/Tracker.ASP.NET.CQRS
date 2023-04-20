using Mapster;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Commands.SecurityCommands.LoginCommand;
using Tracker.Application.Commands.SecurityCommands.RegisterCommand;
using Tracker.Shared.Constants;
using Tracker.Shared.ResponsesDto;
using Tracker.Shared.Security;
using Tracker.WebApi.Common.BaseApiController;
using Tracker.WebApi.Infrastructure.Requests;

namespace Tracker.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SecurityController : BaseApiController<SecurityController>
{
    private readonly IStorage _storage;

    public SecurityController(ILogger<SecurityController> logger, IMediator mediator, IStorage storage)
        : base(mediator, logger)
    {
        _storage = storage;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async ValueTask<IActionResult> LoginAsync(
        [FromBody] LoginEmployeeRequest request,
        CancellationToken cancellationToken
    )
    {
        var mappedRequest = request.Adapt<LoginCommand>();
        await _mediator.Send(mappedRequest, cancellationToken);
        var result = _storage.Get<SecurityResponse>(TrackerOptionsConsts.AUTH_PREFIX_HTTP_ACCESSOR);

        if (result is null)
        {
            _logger.LogInformation("Can not login");
            return BadRequest();
        }

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async ValueTask<IActionResult> RegisterAsync([FromBody] RegisterEmployeeRequest request)
    {
        var mappedRequest = request.Adapt<RegisterCommand>();
        await _mediator.Send(mappedRequest);
        var result = _storage.Get<SecurityResponse>(TrackerOptionsConsts.AUTH_PREFIX_HTTP_ACCESSOR);

        if (result is null)
        {
            _logger.LogInformation("Can not register");
            return BadRequest();
        }

        return Ok(result);
    }
}
