namespace Messaging.Contracts;

public static class ProviderDomainEvents
{
    public sealed record Created
    {
        public required Guid Id { get; init; }
    }

    public sealed record Updated
    {
        public required Guid Id { get; init; }
    }

    public sealed record Deleted
    {
        public required Guid Id { get; init; }
    }
}
