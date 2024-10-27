using Core.Domain.Abstractions.Entities;

namespace Core.Domain.Entities;

public class OrderItem : Entity
{
    public required int Quantity { get; set; }
    public required double Price { get; set; }
    public required string ProductName { get; set; }

    public virtual required Order Order { get; set; }
}
