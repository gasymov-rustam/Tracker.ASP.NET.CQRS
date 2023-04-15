namespace Tracker.Shared.Constants;

public static class TrackerOptionsConsts
{
    public const string CORS_OPTIONS = "Cors";
    public const string RESPONSE_CACHE_OPTIONS = "CacheProfiles";
    public const string RESPONSE_CACHE_MINUTE = "CacheMin";
    public const string RESPONSE_CACHE_DISABLE = "CacheDisable";
    public const string ConsoleOutputTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] {Message}{NewLine}{Exception}";
    public const string FileOutputTemplate = "{Timestamp:HH:mm:ss} [{Level:u3}] ({SourceContext}.{Method}) {Message}{NewLine}{Exception}";
    public const string AppSectionName = "app";
    public const string SerilogSectionName = "serilog";
}