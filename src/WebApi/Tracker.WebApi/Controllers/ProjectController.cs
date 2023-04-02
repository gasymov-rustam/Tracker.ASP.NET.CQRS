using Mediator;
using Microsoft.AspNetCore.Mvc;
using Tracker.Application.Commands.CreateProject;
using Tracker.Application.Commands.DeleteProject;
using Tracker.Application.Commands.UpdateProject;
using Tracker.Application.Queries.GetAllProjects;
using Tracker.Application.Queries.GetSingleProjectById;
using Tracker.Application.Queries.GetTimeTrackingById;
using Tracker.Application.Queries.GetTimeTrackingByWeek;

namespace Tracker.WebApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  // can be removed VaryByQueryKeys
  [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new string[] { "latest" })]
  public class ProjectController : Controller
  {
    private readonly IMediator _mediator;

    public ProjectController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async ValueTask<IActionResult> GetAllProjectsAsync()
    {
      var result = await _mediator.Send(new GetAllProjectsQuery());
      return result is not null ? Ok(result) : NoContent();
    }

    [HttpGet("{id}")]
    public async ValueTask<IActionResult> GetProjectByIdAsync(Guid Id)
    {
      var result = await _mediator.Send(new GetSingleProjectByIdQuery(Id));
      return result is not null ? Ok(result) : NoContent();
    }

    [HttpGet("/ByTime")]
    public async ValueTask<IActionResult> GetTimeTrackingByIdAndDateAsync([FromQuery] GetTimeTrackingByIdAndDateDto data)
    {
      var result = await _mediator.Send(new GetTimeTrackingByIdQuery(data));
      return result is not null ? Ok(result) : NoContent();
    }

    [HttpGet("/ByDate")]
    public async ValueTask<IActionResult> GetTimeTrackingByWeekAsync([FromQuery] GetTimeTrackingByWeekDto data)
    {
      var result = await _mediator.Send(new GetTimeTrackingByWeekQuery(data));
      return result is not null ? Ok(result) : NoContent();
    }

    [HttpPost]
    public async ValueTask<IActionResult> CreateProjectAsync([FromBody] CreateProjectDto data)
    {
      Guid result = await _mediator.Send(new CreateProjectCommand(data));
      return result != Guid.Empty ? Ok(result) : BadRequest();
    }

    [HttpPut("{id}")]
    public async ValueTask<IActionResult> UpdateProjectAsync(Guid id, [FromBody] UpdateProjectDto data)
    {
      Guid result = await _mediator.Send(new UpdateProjectCommand(id, data));
      return result != Guid.Empty ? Ok(result) : BadRequest();
    }

    [HttpDelete("{id}")]
    public async ValueTask<IActionResult> DeleteProjectAsync(Guid id)
    {
      Guid result = await _mediator.Send(new DeleteProjectCommand(id));
      return result != Guid.Empty ? Ok(result) : BadRequest();
    }
  }
}