using Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Tracker.Shared.Constants;

namespace Tracker.Shared.BaseApiController;

[ApiController]
[Route("api/[controller]")]
// [ResponseCache(Duration = 20, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
// [ResponseCache(CacheProfileName = "CacheMin")]
// [ResponseCache(CacheProfileName = TrackerOptionsConsts.RESPONSE_CACHE_MINUTE)]
[ResponseCache(CacheProfileName = TrackerOptionsConsts.RESPONSE_CACHE_DISABLE)]
public abstract class BaseApiController<T> : Controller where T : class
{
    protected readonly ILogger<T> _logger;
    protected readonly IMediator _mediator;

    protected BaseApiController(IMediator mediator, ILogger<T> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
}