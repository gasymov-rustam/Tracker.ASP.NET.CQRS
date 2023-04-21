using FluentValidation;
using Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Application.Common.BaseCommandHandler;
using Tracker.Application.Common.Caching;
using Tracker.Application.Common.Interfaces;
using Tracker.Application.Constants;
using Tracker.Core.Entities;
using Tracker.Shared.Constants;
using Tracker.Shared.Exceptions;
using Tracker.Shared.ResponsesDto;
using Tracker.Shared.Security;

namespace Tracker.Application.Commands.SecurityCommands.LoginCommand;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(4).MaximumLength(16).NotEmpty();

        RuleFor(x => x.Password).MinimumLength(4).MaximumLength(16).NotEmpty();
    }
}

public record LoginCommand(string Name, string Password, string Role) : ICommand<Unit>;

public class LoginCommandHandler : BaseCommandHandler<LoginCommand, Unit>
{
    private readonly IJwtTokenProvider _jwtProvider;
    private readonly IPasswordManager _passwordManager;
    private readonly IStorage _storage;

    public LoginCommandHandler(
        ITrackerDBContext context,
        ILogger<LoginCommand> logger,
        ICacheService cacheService,
        IJwtTokenProvider jwtProvider,
        IPasswordManager passwordManager,
        IStorage storage
    )
        : base(context, logger, cacheService)
    {
        _jwtProvider = jwtProvider ?? throw new ArgumentNullException(nameof(jwtProvider));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
        _storage = storage ?? throw new ArgumentNullException(nameof(storage));
    }

    public async override ValueTask<Unit> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("LoginCommandHandler");

        var id = TrackerApplicationConsts.USER_REDIS_PREFIX + command.Name;

        var existedUser = await _cacheService.GetAsync(
            id,
            async () =>
            {
                var employee = await _context.Users.FirstOrDefaultAsync(x => x.Name == command.Name, cancellationToken);

                if (employee is null)
                {
                    _logger.LogError("GetEmployeeByIdHandler: user with Name {Name} does not exist", command.Name);
                    return null;
                }

                _logger.LogDebug(employee.ToString());

                return employee;
            },
            cancellationToken
        );

        if (existedUser is null)
        {
            _logger.LogError("LoginCommandHandler: user not found");
            throw new BaseException(ExceptionCodes.ValueMismatch, "LoginCommandHandler: user not found");
        }

        bool isCorrectPassword = _passwordManager.Validate(command.Password, existedUser.Password);

        if (!isCorrectPassword)
        {
            _logger.LogError("LoginCommandHandler: password is not correct");
            throw new BaseException(ExceptionCodes.ValueMismatch, "LoginCommandHandler: password is not correct");
        }

        var jwt = _jwtProvider.CreateToken(existedUser.Id.ToString(), existedUser.Name, null);

        var role = await _context.Roles.FirstOrDefaultAsync(x => x.Id == existedUser.RoleId, cancellationToken);

        var authResponse = new SecurityResponse(
            existedUser.Id,
            existedUser.Name,
            role?.Name ?? Role.DefaultName,
            jwt.AccessToken,
            jwt.Expires
        );

        _storage.Set(authResponse, TrackerOptionsConsts.AUTH_PREFIX_HTTP_ACCESSOR);

        return Unit.Value;
    }
}
