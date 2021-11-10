namespace BusinessLogic.Infrastructure
{
    public record Error(string Id, ErrorCode ErrorCode, string CorrelationId, object data = null) { }
}