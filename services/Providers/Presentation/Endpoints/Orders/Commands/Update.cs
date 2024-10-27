using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Core.Application.Commands.Update;

namespace Presentation.Endpoints.Commands;

public static class Update
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Requests.Order, IResult>
    {
        public override void Configure()
        {
            Put("/update");
            Version(1);
            Options(x => x.WithTags("Commands"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Order request, CancellationToken ct)
        {
            var command = new Command(request);

            var response = await sender.Send(command, ct);

            return TypedResults.Ok(response);
        }
    }
}