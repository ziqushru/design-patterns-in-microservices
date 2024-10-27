using Contracts.Core.Domain.Abstractions.Entities;
using Contracts.Core.Domain.Enums;

namespace Contracts.Core.Domain.Entities;

public class Consumer : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required ConsumerType Type { get; set; }
}
