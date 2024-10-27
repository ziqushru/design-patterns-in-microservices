using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Presentation.Responses;
using Contracts.Presentation.Utils;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Contracts.Presentation.PostProcessors;

public class ExceptionPostProcessor(ILogger logger) : IGlobalPostProcessor
{
    public async Task PostProcessAsync(IPostProcessorContext context, CancellationToken ct)
    {
        if (!context.HasExceptionOccurred)
            return;

        var exception = context.ExceptionDispatchInfo.SourceException;

        var statusCode = HttpContextUtils.GetStatusCode(exception);

        if (statusCode != StatusCodes.Status422UnprocessableEntity)
        {
            var request = context.HttpContext.Request;
            var uriString = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}";

            var endpointUri = new Uri(uriString);

            var exceptionMessage = context.ExceptionDispatchInfo.SourceException.Message;

            logger.Error(
                "An exception occurred while processing the request to {EndpointUri}. Exception: {ExceptionMessage}",
                endpointUri.PathAndQuery,
                exceptionMessage);
        }

        var response = new ExceptionResponse(
            statusCode,
            HttpContextUtils.GetTitle(exception),
            exception.Message,
            HttpContextUtils.GetErrors(exception));

        context.HttpContext.Response.ContentType = "application/json";
        context.HttpContext.Response.StatusCode = statusCode;

        await context.HttpContext.Response.SendAsync(
            response,
            statusCode,
            cancellation: ct);

        context.MarkExceptionAsHandled();
    }
}
