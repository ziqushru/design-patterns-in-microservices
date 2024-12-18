using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Consumers.Core.Application.Abstractions.Commands;
using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Application.Abstractions.Services;
using Consumers.Core.Domain.Abstractions.Exceptions;
using Consumers.Core.Domain.Entities;
using Consumers.Core.Domain.Enums;
using FluentValidation;

namespace Consumers.Core.Application.Commands;

public static class Update
{
    public static class Requests
    {
        public sealed record Consumer
        {
            public required Guid Id { get; init; }
            public required string FirstName { get; init; }
            public required string LastName { get; init; }
            public required string Email { get; init; }
            public required string CellPhone { get; init; }
            public required ConsumerType Type { get; init; }
        }
    }

    public sealed record Command(Requests.Consumer Consumer) : IAppCommandWithoutResponse;

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Requests.Consumer, Consumer>();
        }
    }

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Consumer)
                .NotEmpty()
                .WithMessage("Ο καταναλωτής είναι υποχρεωτικός");

            RuleFor(command => command.Consumer.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");

            RuleFor(command => command.Consumer.FirstName)
                .NotEmpty()
                .WithMessage("Το όνομα είναι υποχρεωτικό");

            RuleFor(command => command.Consumer.LastName)
                .NotEmpty()
                .WithMessage("Το επώνυμο είναι υποχρεωτικό");

            RuleFor(command => command.Consumer.Email)
                .EmailAddress()
                .WithMessage("Το email είναι υποχρεωτικό");

            RuleFor(command => command.Consumer.CellPhone)
                .Matches("^[0-9]*$")
                .WithMessage("Το κινητό τηλέφωνο είναι υποχρεωτικό");

            RuleFor(command => command.Consumer.Type)
                .NotEmpty()
                .WithMessage("Ο τύπος καταναλωτή είναι υποχρεωτικός");
        }
    }

    internal sealed class CommandHandler(
        IMapper mapper,
        IConsumersRepository consumersRepository,
        IConsumersService consumersService)
        : IAppCommandHandlerWithoutResponse<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var consumer = await consumersRepository.GetByIdAsync(command.Consumer.Id, cancellationToken)
                ?? throw new NotFoundException("Δεν βρέθηκε o καταναλωτής");

            mapper.Map(command.Consumer, consumer);

            await consumersService.UpdateAsync(consumer, cancellationToken);
        }
    }
}
