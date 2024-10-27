using System;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Application.Abstractions.Commands;
using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Application.Abstractions.Services;
using FastEndpoints;
using FluentValidation;

namespace Consumers.Core.Application.Commands;

public static class Delete
{
    public static class Requests
    {
        public sealed record Consumer
        {
            [FromBody]
            public required Guid Id { get; init; }
        }
    }

    public sealed record Command(Requests.Consumer Consumer) : IAppCommandWithoutResponse;

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Consumer.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class CommandHandler(
        IConsumersService consumersService,
        IConsumersRepository consumersRepository)
        : IAppCommandHandlerWithoutResponse<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var consumer = await consumersRepository.GetByIdAsync(command.Consumer.Id, cancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο καταναλωτής");

            await consumersService.DeleteAsync(consumer, cancellationToken);
        }
    }
}
