using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Consumers.Core.Application.Commands.Create;

namespace Consumers.Presentation.Endpoints.Orders.Commands;

public static class Create
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Core.Application.Commands.Create.Requests.Consumer, IResult>
    {
        public override void Configure()
        {
            Post("/create");
            Version(1);
            Options(x => x.WithTags("Commands"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Consumer request, CancellationToken ct)
        {
            var command = new Command(request);

            var response = await sender.Send(command, ct);

            return TypedResults.Ok(response);
        }
    }
}
