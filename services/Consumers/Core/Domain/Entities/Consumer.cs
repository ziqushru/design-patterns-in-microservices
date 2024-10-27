using Consumers.Core.Domain.Abstractions.Entities;
using Consumers.Core.Domain.Enums;

namespace Consumers.Core.Domain.Entities;

public class Consumer : Entity
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string CellPhone { get; set; }
    public required ConsumerType Type { get; set; }
}
