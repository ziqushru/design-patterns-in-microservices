using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ProviderDeleted
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Deleted, Provider>();
        }
    }

    public sealed class MessageConsumer(
        IProvidersRepository providersRepository,
        IMapper mapper)
        : IConsumer<Deleted>
    {
        public async Task Consume(ConsumeContext<Deleted> context)
        {
            var provider = await providersRepository.GetByIdAsync(context.Message.Id, context.CancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο πάροχος");

            await providersRepository.DeleteAsync(provider, context.CancellationToken);
        }
    }
}
