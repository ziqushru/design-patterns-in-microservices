using Providers.Core.Domain.Abstractions.Exceptions;

namespace Providers.Core.Domain.Exceptions;

public sealed class ReferencedDataDeleteException(string entityName)
    : InvalidOperationException("Cannot delete " + entityName + " entity with related child entities.");
