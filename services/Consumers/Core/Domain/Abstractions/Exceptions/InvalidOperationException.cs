namespace Consumers.Core.Domain.Abstractions.Exceptions;

public abstract class InvalidOperationException(string message)
    : ApplicationException("Invalid Operation", message);
