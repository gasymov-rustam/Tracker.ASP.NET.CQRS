using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Core.Entities;
using Tracker.Shared.Security;

namespace Tracker.Infrastructure.Dal.Initializers;

public class UserInitiallizer : IDataInitializer
{
    private readonly TrackerDbContext _dbContext;
    private readonly ILogger<UserInitiallizer> _logger;
    private readonly IPasswordManager _passwordManager;

    public UserInitiallizer(
        TrackerDbContext dbContext,
        ILogger<UserInitiallizer> logger, IPasswordManager passwordManager)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
    }

    public async Task InitAsync()
    {
        if (_dbContext.Users.Any()) return;

        await AddUserAsync();

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddUserAsync()
    {
        await _dbContext.Users.AddAsync(new User("Alex", _passwordManager.Secure("111111111111")));

        _logger.LogInformation("Initialized UsersDb");
    }
}
