namespace Tracker.Shared.Security.Time;

public interface IUtcClock
{
    DateTime GetCurrentUtc();
}

public class UtcClock : IUtcClock
{
    public DateTime GetCurrentUtc() => DateTime.UtcNow;
}