using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ProviderCreated
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Created, Provider>();
        }
    }

    public sealed class MessageConsumer(
        IProvidersRepository providersRepository,
        IMapper mapper)
        : IConsumer<Created>
    {
        public async Task Consume(ConsumeContext<Created> context)
        {
            var provider = mapper.Map<Provider>(context.Message);

            await providersRepository.CreateAsync(provider, context.CancellationToken);
        }
    }
}
