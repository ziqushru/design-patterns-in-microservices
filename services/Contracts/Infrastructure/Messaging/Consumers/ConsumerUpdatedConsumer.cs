using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ConsumerUpdated
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Updated, Consumer>();
        }
    }

    public sealed class MessageConsumer(
        IConsumersRepository consumersRepository,
        IMapper mapper)
        : IConsumer<Updated>
    {
        public async Task Consume(ConsumeContext<Updated> context)
        {
            var consumer = await consumersRepository.GetByIdAsync(context.Message.Id, context.CancellationToken)
                ?? throw new Exception("Δεν βρέθηκε ο καταναλωτής");

            mapper.Map(context.Message, consumer);

            await consumersRepository.UpdateAsync(consumer, context.CancellationToken);
        }
    }
}
