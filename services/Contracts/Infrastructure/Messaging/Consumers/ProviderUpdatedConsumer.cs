using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ProviderUpdated
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Updated, Provider>();
        }
    }

    public sealed class MessageConsumer(
        IProvidersRepository providersRepository,
        IMapper mapper)
        : IConsumer<Updated>
    {
        public async Task Consume(ConsumeContext<Updated> context)
        {
            var provider = await providersRepository.GetByIdAsync(context.Message.Id, context.CancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο πάροχος");

            mapper.Map(context.Message, provider);

            await providersRepository.UpdateAsync(provider, context.CancellationToken);
        }
    }
}
