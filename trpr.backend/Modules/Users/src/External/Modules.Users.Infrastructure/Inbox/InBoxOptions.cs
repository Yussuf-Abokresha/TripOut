namespace Modules.Users.Infrastructure.Inbox;

public class InBoxOptions
{
    public bool Enabled { get; set; }
    public int BatchSize { get; set; }
    public int TimeSpanInSeconds { get; set; }
}
