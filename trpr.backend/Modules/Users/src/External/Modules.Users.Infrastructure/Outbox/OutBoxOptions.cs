namespace Modules.Users.Infrastructure.Outbox;

public class OutBoxOptions
{
    public bool Enabled { get; set; }
    public int BatchSize { get; set; }
    public int TimeSpanInSeconds { get; set; }
}
