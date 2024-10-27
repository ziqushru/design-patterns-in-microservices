using System.Collections.Generic;

namespace Contracts.Presentation.Responses;

public sealed record ExceptionResponse(
    int Status,
    string Title,
    string Detail,
    IDictionary<string, string[]>? Errors);
