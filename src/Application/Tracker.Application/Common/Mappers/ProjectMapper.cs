using Mapster;
using Tracker.Application.Commands.CreateProject;
using Tracker.Application.Queries.GetSingleProjectById;
using Tracker.Core.Entities;

namespace Tracker.Application.Common.Mappers;

public class ProjectMapper : IRegister
{
  public void Register(TypeAdapterConfig config)
  {
    config.NewConfig<CreateProjectDto, Project>().RequireDestinationMemberSource(true);
    config.NewConfig<Project, GetSingleProjectDto>().RequireDestinationMemberSource(true);
  }
}