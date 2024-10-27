using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Core.Application.Queries.GetById;

namespace Presentation.Endpoints.Queries;

public static class GetById
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Requests.Order, IResult>
    {
        public override void Configure()
        {
            Get("/get-by-id");
            Version(1);
            Options(x => x.WithTags("Queries"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Order request, CancellationToken ct)
        {
            var query = new Query(request);

            var response = await sender.Send(query, ct);

            return TypedResults.Ok(response);
        }
    }
}
