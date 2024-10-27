using System.Collections.Generic;
using Contracts.Core.Domain.Abstractions.Exceptions;
using FluentValidation.Results;

namespace Contracts.Core.Domain.Exceptions;

public sealed class ValidationException(IList<ValidationFailure> errors)
    : ApplicationException("Validation Failure", "One or more validation errors occurred")
{
    public IList<ValidationFailure> Errors { get; } = errors;
}
