using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Contracts.Core.Application.Abstractions.Commands;
using Contracts.Core.Application.Abstractions.Services;
using Contracts.Core.Domain.Entities;
using Contracts.Core.Domain.Enums;
using FluentValidation;

namespace Contracts.Core.Application.Commands;

public static class Create
{
    public static class Requests
    {
        public sealed record Contract
        {
            public required Guid ConsumerId { get; init; }
            public required Guid ProviderId { get; init; }
            public required ContractStatus Status { get; init; }
        }
    }

    public static class Responses
    {
        public sealed record Contract
        {
            public required Guid Id { get; init; }
        }
    }

    public sealed record Command(Requests.Contract Contract) : IAppCommand<Responses.Contract>;

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
        IContractsService contractsService)
        : IAppCommandHandler<Command, Responses.Contract>
    {
        public async Task<Responses.Contract> Handle(Command command, CancellationToken cancellationToken)
        {
            var contract = mapper.Map<Contract>(command.Contract);

            var id = await contractsService.CreateAsync(contract, cancellationToken);

            return new Responses.Contract
            {
                Id = id
            };
        }
    }
}
