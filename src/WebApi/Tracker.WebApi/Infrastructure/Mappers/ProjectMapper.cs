using Mapster;
using Tracker.Application.Commands.CreateProject;
using Tracker.Application.Commands.RoleCommands.UpdateRoleCommand;
using Tracker.Application.Commands.UpdateProject;
using Tracker.WebApi.Infrastructure.Requests;

namespace Tracker.WebApi.Infrastructure.Mappers;

public class ProjectMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateProjectRequest, CreateProjectCommand>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<UpdateProjectRequest, UpdateProjectCommand>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<UpdateRoleRequest, UpdateRoleCommand>()
            .RequireDestinationMemberSource(true);
    }
}