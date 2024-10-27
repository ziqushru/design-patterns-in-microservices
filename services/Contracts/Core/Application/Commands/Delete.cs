using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Core.Application.Abstractions.Commands;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Application.Abstractions.Services;
using FastEndpoints;
using FluentValidation;

namespace Contracts.Core.Application.Commands;

public static class Delete
{
    public static class Requests
    {
        public sealed record Contract
        {
            [FromBody]
            public required Guid Id { get; init; }
        }
    }

    public sealed record Command(Requests.Contract Contract) : IAppCommandWithoutResponse;

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Contract.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class CommandHandler(
        IContractsService contractsService,
        IContractsRepository contractsRepository)
        : IAppCommandHandlerWithoutResponse<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = await contractsRepository.GetByIdAsync(command.Contract.Id, cancellationToken)
                ?? throw new Exception("Δεν βρέθηκε η σύμβαση");

            await contractsService.DeleteAsync(contract, cancellationToken);
        }
    }
}
