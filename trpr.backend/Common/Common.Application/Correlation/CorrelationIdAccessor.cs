namespace Common.Application.Correlation;

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private static readonly AsyncLocal<string?> _correlationId = new();

    public string? CorrelationId
    {
        get => _correlationId.Value;
        set => _correlationId.Value = value;
    }
}