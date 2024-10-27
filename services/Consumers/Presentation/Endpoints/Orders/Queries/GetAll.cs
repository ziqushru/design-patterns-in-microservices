using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Consumers.Core.Application.Queries.GetAll;

namespace Consumers.Presentation.Endpoints.Orders.Queries;

public static class GetAll
{
    internal sealed class Endpoint(ISender sender) : EndpointWithoutRequest<IResult>
    {
        public override void Configure()
        {
            Get("/get-all");
            Version(1);
            Options(x => x.WithTags("Queries"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(CancellationToken ct)
        {
            var query = new Query();

            var response = await sender.Send(query, ct);

            return TypedResults.Ok(response);
        }
    }
}
