using Mapster;
using Tracker.Application.Commands.CreateProject;
using Tracker.Application.Commands.EmployeeCommands.CreateEmployeeCommand;
using Tracker.Application.Commands.EmployeeCommands.UpdateEmployeeCommand;
using Tracker.Application.Commands.RoleCommands.UpdateRoleCommand;
using Tracker.Application.Commands.SecurityCommands.LoginCommand;
using Tracker.Application.Commands.SecurityCommands.RegisterCommand;
using Tracker.Application.Commands.UpdateProject;
using Tracker.WebApi.Infrastructure.Requests;

namespace Tracker.WebApi.Infrastructure.Mappers;

public class ProjectMapper : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<CreateProjectRequest, CreateProjectCommand>()
            .RequireDestinationMemberSource(true);

        config
            .NewConfig<UpdateProjectRequest, UpdateProjectCommand>()
            .RequireDestinationMemberSource(true);

        config
            .NewConfig<UpdateRoleRequest, UpdateRoleCommand>()
            .RequireDestinationMemberSource(true);

        config
            .NewConfig<CreateEmployeeRequest, CreateEmployeeCommand>()
            .RequireDestinationMemberSource(true);

        config
            .NewConfig<UpdateEmployeeRequest, UpdateEmployeeCommand>()
            .RequireDestinationMemberSource(true);

        config.NewConfig<LoginEmployeeRequest, LoginCommand>().RequireDestinationMemberSource(true);

        config
            .NewConfig<RegisterEmployeeRequest, RegisterCommand>()
            .RequireDestinationMemberSource(true);
    }
}
