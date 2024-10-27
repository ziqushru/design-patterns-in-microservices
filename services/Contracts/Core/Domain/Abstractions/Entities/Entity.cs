using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace Contracts.Core.Domain.Abstractions.Entities;

public interface IDomainEvent : INotification { }

public abstract class Entity
{
    public Guid Id { get; init; }

    [NotMapped]
    public List<IDomainEvent> DomainEvents { get; } = [];

    public void QueueDomainEvent(IDomainEvent @event) => DomainEvents.Add(@event);
}
