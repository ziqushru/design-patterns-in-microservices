using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Presentation.Responses;
using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace Consumers.Presentation.PreProcessors;

public class ExceptionPreProcessor : IGlobalPreProcessor
{
    public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
        if (!ctx.HasValidationFailures)
            return;

        var statusCode = StatusCodes.Status422UnprocessableEntity;

        var response = new ExceptionResponse(
            statusCode,
            "Validation Error",
            "One or more validation failures occurred.",
            ctx.ValidationFailures.ToDictionary(x => x.PropertyName, x => new[] { x.ErrorMessage }));

        ctx.HttpContext.Response.ContentType = "application/json";
        ctx.HttpContext.Response.StatusCode = statusCode;

        await ctx.HttpContext.Response.SendAsync(
            response,
            statusCode,
            cancellation: ct);
    }
}
