using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Providers.Core.Application.Abstractions.Commands;
using Providers.Core.Application.Abstractions.Services;
using Providers.Core.Domain.Entities;
using FluentValidation;

namespace Providers.Core.Application.Commands;

public static class Create
{
    public static class Requests
    {
        public sealed record Provider
        {
            public required string BrandName { get; init; }
            public required string Vat { get; init; }
            public required string Email { get; init; }
        }
    }

    public static class Responses
    {
        public sealed record Provider
        {
            public required Guid Id { get; init; }
        }
    }

    public sealed record Command(Requests.Provider Provider) : IAppCommand<Responses.Provider>;

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Requests.Provider, Provider>();
        }
    }

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Provider)
                .NotEmpty()
                .WithMessage("Ο πάροχος είναι υποχρεωτικός");

            RuleFor(command => command.Provider.BrandName)
                .NotEmpty()
                .WithMessage("Η επωνυμία είναι υποχρεωτική");

            RuleFor(command => command.Provider.Vat)
                .NotEmpty()
                .WithMessage("Το ΑΦΜ είναι υποχρεωτικό");

            RuleFor(command => command.Provider.Email)
                .EmailAddress()
                .WithMessage("Το email είναι υποχρεωτικό");
        }
    }

    internal sealed class CommandHandler(
        IMapper mapper,
        IProvidersService providersService)
        : IAppCommandHandler<Command, Responses.Provider>
    {
        public async Task<Responses.Provider> Handle(Command command, CancellationToken cancellationToken)
        {
            var provider = mapper.Map<Provider>(command.Provider);

            var id = await providersService.CreateAsync(provider, cancellationToken);

            return new Responses.Provider
            {
                Id = id
            };
        }
    }
}
