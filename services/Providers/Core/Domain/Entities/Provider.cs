using Providers.Core.Domain.Abstractions.Entities;

namespace Providers.Core.Domain.Entities;

public class Provider : Entity
{
    public required string BrandName { get; set; }
    public required string Vat { get; set; }
    public required string Email { get; set; }
}
