using Core.Domain.Enums;

namespace Core.Domain.ValueObjects;

public record Transition(Status SourceStatus, Status DestinationStatus);
