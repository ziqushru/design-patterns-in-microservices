using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Contracts.Core.Application.Commands.Update;

namespace Contracts.Presentation.Endpoints.Orders.Commands;

public static class Update
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Core.Application.Commands.Update.Requests.Contract, IResult>
    {
        public override void Configure()
        {
            Put("/update");
            Version(1);
            Options(x => x.WithTags("Commands"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Contract request, CancellationToken ct)
        {
            var command = new Command(request);

            await sender.Send(command, ct);

            return TypedResults.Ok();
        }
    }
}
