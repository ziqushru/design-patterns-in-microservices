using System;
using System.Collections.Generic;
using Core.Domain.Abstractions.Entities;
using Core.Domain.Enums;

namespace Core.Domain.Entities;

public class Order : Entity
{
    public required Stock Stock { get; set; }
    public required Status Status { get; set; }
    public required DateTime DateSubmitted { get; set; }
    public required double Price { get; set; }
    public required int OrderNumber { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string CellPhone { get; set; }
    public required string StreetName { get; set; }
    public required string StreetNumber { get; set; }
    public required string ZipCode { get; set; }
    public required string Town { get; set; }
    public required string Country { get; set; }
    public required string ShippingMethodName { get; set; }
    public required string PaymentMethodName { get; set; }
    public string? TrackingNumber { get; set; }
    public string? Note { get; set; }

    public virtual required ICollection<OrderItem> OrderItems { get; set; } = [];
}
