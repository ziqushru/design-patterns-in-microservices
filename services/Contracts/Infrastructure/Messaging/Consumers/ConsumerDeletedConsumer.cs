using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ConsumerDeleted
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Deleted, Consumer>();
        }
    }

    public sealed class MessageConsumer(
        IConsumersRepository consumersRepository,
        IMapper mapper)
        : IConsumer<Deleted>
    {
        public async Task Consume(ConsumeContext<Deleted> context)
        {
            var consumer = await consumersRepository.GetByIdAsync(context.Message.Id, context.CancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο καταναλωτής");

            await consumersRepository.DeleteAsync(consumer, context.CancellationToken);
        }
    }
}
