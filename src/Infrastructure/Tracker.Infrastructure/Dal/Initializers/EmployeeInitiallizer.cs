using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tracker.Core.Entities;

namespace Tracker.Infrastructure.Dal.Initializers;

public class EmployeeInitiallizer : IDataInitializer
{
    private readonly TrackerDbContext _dbContext;
    private readonly ILogger<EmployeeInitiallizer> _logger;

    public EmployeeInitiallizer(
        TrackerDbContext dbContext,
        ILogger<EmployeeInitiallizer> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InitAsync()
    {
        if (_dbContext.Employees.Any()) return;

        await AddEmployeeAsync();

        await _dbContext.SaveChangesAsync();
    }

    private async Task AddEmployeeAsync()
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync();

        if (role is null)
            throw new Exception("RoleDb is empty");

        await _dbContext.Employees.AddAsync(new Employee("Alex", "man", new DateOnly(2020, 01, 10), role.Id));

        _logger.LogInformation("Initialized employees");
    }
}
