using Contracts.Core.Domain.Abstractions.Exceptions;

namespace Contracts.Core.Domain.Exceptions;

public sealed class ReferencedDataDeleteException(string entityName)
    : InvalidOperationException("Cannot delete " + entityName + " entity with related child entities.");
