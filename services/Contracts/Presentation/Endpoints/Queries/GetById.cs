using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Contracts.Core.Application.Queries.GetById;

namespace Contracts.Presentation.Endpoints.Orders.Queries;

public static class GetById
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Core.Application.Queries.GetById.Requests.Contract, IResult>
    {
        public override void Configure()
        {
            Get("/get-by-id");
            Version(1);
            Options(x => x.WithTags("Queries"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Contract request, CancellationToken ct)
        {
            var query = new Query(request);

            var response = await sender.Send(query, ct);

            return TypedResults.Ok(response);
        }
    }
}
