using System;
using System.Collections.Generic;
using System.Linq;
using Providers.Core.Domain.Abstractions.Exceptions;
using Providers.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using ApplicationException = Providers.Core.Domain.Abstractions.Exceptions.ApplicationException;

namespace Providers.Presentation.Utils;

public static class HttpContextUtils
{
    public static string GetRequestUri(this HttpContext httpContext)
    {
        var request = httpContext.Request;

        return $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";
    }

    public static int GetStatusCode(Exception exception) =>
        exception switch
        {
            BadRequestException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status422UnprocessableEntity,
            _ => StatusCodes.Status500InternalServerError
        };

    public static string GetTitle(Exception exception) =>
        exception switch
        {
            // TODO: Add more exception types
            ApplicationException applicationException => applicationException.Title,
            _ => "Server Error"
        };

    public static IDictionary<string, string[]>? GetErrors(Exception exception)
    {
        IDictionary<string, string[]>? errors = null;

        if (exception is ValidationException validationException)
            errors = validationException.Errors.ToDictionary(x =>
                x.PropertyName,
                x => new[] { x.ErrorMessage });

        return errors;
    }
}
