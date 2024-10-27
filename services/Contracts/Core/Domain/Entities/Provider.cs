using Contracts.Core.Domain.Abstractions.Entities;
using Contracts.Core.Domain.Enums;

namespace Contracts.Core.Domain.Entities;

public class Provider : Entity
{
    public required string BrandName { get; set; }
}
