namespace Core.Domain.Abstractions.Exceptions;

public class NotFoundException(string message) : ApplicationException("Not Found", message);
