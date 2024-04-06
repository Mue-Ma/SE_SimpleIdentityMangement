namespace EventService.Server.Persistence
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; init; } = string.Empty;
        public string DatabaseName { get; init; } = string.Empty;
    }
}