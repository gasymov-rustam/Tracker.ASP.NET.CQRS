using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Constants;
using Tracker.Core.Entities;
using Tracker.Shared.ResponsesDto;
using Tracker.Shared.Security;

namespace Tracker.Application.Commands.SecurityCommands.RegisterCommand;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(8).MaximumLength(16).NotEmpty();

        RuleFor(x => x.Password).MinimumLength(8).MaximumLength(16).NotEmpty();
    }
}

public record RegisterCommand(string Name, string Password) : ICommand<Unit>;

public class RegisterCommandHandler : BaseCommandHandler<RegisterCommand, Unit>
{
    private readonly IJwtTokenProvider _jwtProvider;
    private readonly IPasswordManager _passwordManager;
    private readonly IStorage _storage;

    public RegisterCommandHandler(
        ITrackerDBContext context,
        ILogger<RegisterCommand> logger,
        ICacheService cacheService,
        IJwtTokenProvider jwtProvider,
        IPasswordManager passwordManager,
        IStorage storage
    )
        : base(context, logger, cacheService)
    {
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _passwordManager =
            passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async override ValueTask<Unit> Handle(
        RegisterCommand command,
        CancellationToken cancellationToken
    )
    {
        _logger.LogInformation("RegisterCommandHandler");

        var id = TrackerApplicationConsts.USER_REDIS_PREFIX + command.Name;

        var existedUser = await _cacheService.GetAsync(
            id,
            async () =>
            {
                var employee = await _context.Users.FirstOrDefaultAsync(
                    x => x.Name == command.Name,
                    cancellationToken
                );

                if (employee is null)
                {
                    _logger.LogError(
                        "GetEmployeeByIdHandler: employee with Name {Name} does not exist",
                        command.Name
                    );
                    return null;
                }

                _logger.LogDebug(employee.ToString());

                return employee;
            },
            cancellationToken
        );

        if (existedUser is not null)
        {
            _logger.LogError("LoginCommandHandler: user with this name already exists");
            return Unit.Value;
        }

        User user = new(command.Name, _passwordManager.Secure(command.Password));

        var createdUser = await _context.Users.AddAsync(user, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        var jwt = _jwtProvider.CreateToken(
            createdUser.Entity.Id.ToString(),
            createdUser.Entity.Name,
            null
        );

        var authResponse = new SecurityResponse(
            createdUser.Entity.Id,
            createdUser.Entity.Name,
            jwt.AccessToken,
            jwt.Expires
        );

        _storage.Set(authResponse, "auth");

        var createdUserId = TrackerApplicationConsts.USER_REDIS_PREFIX + createdUser.Entity.Name;

        await _cacheService.SetAsync(createdUserId, createdUser, cancellationToken);

        return Unit.Value;
    }
}
