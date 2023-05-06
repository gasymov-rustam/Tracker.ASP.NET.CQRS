using Microsoft.Extensions.Logging;
using Tracker.Application.Commands.RoleCommands.CreateRole;
using Tracker.Application.Common.Interfaces;
using Tracker.Shared.Exceptions;

namespace Tracker.Tests.Application.RoleTests;
public class CreateRoleTests
{
    private readonly Mock<ITrackerDBContext> _context;
    private readonly Mock<ILogger<CreateRoleHandler>> _logger;

    public CreateRoleTests()
    {
        _context = new();
        _logger = new();
    }

    [Fact]
    public async ValueTask CREATE_ROLE_SHOULD_BE_SUCCESS()
    {
        var command = new CreateRoleCommand("Admin");

        var handler = new CreateRoleHandler(_context.Object, _logger.Object);

        Guid result = await handler.Handle(command, default);

        Assert.IsType<Guid>(result);
        Assert.True(result != Guid.Empty);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public async ValueTask CREATE_ROLE_SHOULD_BE_FAULT_VALIDATION_ERROR()
    {
        var command = new CreateRoleCommand("");

        var handler = new CreateRoleHandler(_context.Object, _logger.Object);

        Guid result = await handler.Handle(command, default);

        Assert.IsType<ValidationError>(result);
        result.Should().As<ValidationError>();
    }
}

