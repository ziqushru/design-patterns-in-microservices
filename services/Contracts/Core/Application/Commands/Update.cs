using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Core.Application.Abstractions.Commands;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Application.Abstractions.Services;
using Contracts.Core.Domain.Abstractions.Exceptions;
using Contracts.Core.Domain.Entities;
using Contracts.Core.Domain.Enums;
using FluentValidation;

namespace Contracts.Core.Application.Commands;

public static class Update
{
    public static class Requests
    {
        public sealed record Contract
        {
            public required Guid Id { get; init; }
            public required Guid ConsumerId { get; init; }
            public required Guid ProviderId { get; init; }
            public required ContractStatus Status { get; init; }
        }
    }

    public sealed record Command(Requests.Contract Contract) : IAppCommandWithoutResponse;

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Requests.Contract, Contract>();
        }
    }

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Contract)
                .NotEmpty()
                .WithMessage("Η σύμβαση είναι υποχρεωτική");

            RuleFor(command => command.Contract.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");

            RuleFor(command => command.Contract.ConsumerId)
                .NotEmpty()
                .WithMessage("Το ConsumerId είναι υποχρεωτικό");

            RuleFor(command => command.Contract.ProviderId)
                .NotEmpty()
                .WithMessage("Το ProviderId είναι υποχρεωτικό");

            RuleFor(command => command.Contract.Status)
                .NotEmpty()
                .WithMessage("Η κατάσταση της σύμβασης είναι υποχρεωτική");
        }
    }

    internal sealed class CommandHandler(
        IMapper mapper,
        IContractsRepository contractsRepository,
        IContractsService contractsService)
        : IAppCommandHandlerWithoutResponse<Command>
    {
        public async Task Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = await contractsRepository.GetByIdAsync(command.Contract.Id, cancellationToken)
                ?? throw new NotFoundException("Δεν βρέθηκε η σύμβαση");

            mapper.Map(command.Contract, contract);

            await contractsService.UpdateAsync(contract, cancellationToken);
        }
    }
}
