using System;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Application.Abstractions.Commands;
using Providers.Core.Application.Abstractions.Repositories;
using Providers.Core.Application.Abstractions.Services;
using FastEndpoints;
using FluentValidation;

namespace Providers.Core.Application.Commands;

public static class Delete
{
    public static class Requests
    {
        public sealed record Provider
        {
            [FromBody]
            public required Guid Id { get; init; }
        }
    }

    public sealed record Command(Requests.Provider Provider) : IAppCommandWithoutResponse;

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Provider.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class CommandHandler(
        IProvidersService providersService,
        IProvidersRepository providersRepository)
        : IAppCommandHandlerWithoutResponse<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var provider = await providersRepository.GetByIdAsync(command.Provider.Id, cancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο πάροχος");

            await providersService.DeleteAsync(provider, cancellationToken);
        }
    }
}
