namespace Core.Domain.Abstractions.Exceptions;

public abstract class BadRequestException(string message) : ApplicationException("Bad Request", message);
