using System.ComponentModel;

namespace Messaging.Contracts;

public static class ConsumerDomainEvents
{
    public sealed record Created
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required int Type { get; init; }
    }

    public sealed record Updated
    {
        public required Guid Id { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required int Type { get; init; }
    }

    public sealed record Deleted
    {
        public required Guid Id { get; init; }
    }
}