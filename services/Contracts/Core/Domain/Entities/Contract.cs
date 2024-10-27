using System;
using Contracts.Core.Domain.Abstractions.Entities;
using Contracts.Core.Domain.Enums;

namespace Contracts.Core.Domain.Entities;

public class Contract : Entity
{
    public required Guid ConsumerId { get; set; }
    public required Guid ProviderId { get; set; }
    public required ContractStatus Status { get; set; }
}
