using Microsoft.Extensions.Logging;

namespace Tracker.Infrastructure.Dal.Initializers;

public class RoleInitiallizer : IDataInitializer
{
    private readonly TrackerDbContext _dbContext;
    private readonly ILogger<RoleInitiallizer> _logger;

    public RoleInitiallizer(
        TrackerDbContext dbContext,
        ILogger<RoleInitiallizer> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitAsync()
    {
        if (_dbContext.Roles.Any()) return;

        await AddRolesAsync();

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddRolesAsync()
    {
        await _dbContext.Roles.AddAsync(new Core.Entities.Role("employee"));

        _logger.LogInformation("Initialized roles");
    }
}
