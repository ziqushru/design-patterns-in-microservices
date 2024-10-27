using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using static Providers.Core.Application.Commands.Delete;

namespace Providers.Presentation.Endpoints.Orders.Commands;

public static class Delete
{
    internal sealed class Endpoint(ISender sender) : Endpoint<Core.Application.Commands.Delete.Requests.Provider, IResult>
    {
        public override void Configure()
        {
            Delete("/delete");
            Version(1);
            Options(x => x.WithTags("Commands"));
            AllowAnonymous();
        }

        public override async Task<IResult> ExecuteAsync(Requests.Provider request, CancellationToken ct)
        {
            var command = new Command(request);

            await sender.Send(command, ct);

            return TypedResults.Ok();
        }
    }
}
