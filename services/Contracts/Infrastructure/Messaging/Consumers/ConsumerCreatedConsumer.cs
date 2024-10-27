using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;
using MassTransit;
using static Messaging.Contracts.ConsumerDomainEvents;

namespace Contracts.MessageBus.Consumers;

public static class ConsumerCreated
{
    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Created, Consumer>();
        }
    }

    public sealed class MessageConsumer(
        IConsumersRepository consumersRepository,
        IMapper mapper)
        : IConsumer<Created>
    {
        public async Task Consume(ConsumeContext<Created> context)
        {
            var consumer = mapper.Map<Consumer>(context.Message);

            await consumersRepository.CreateAsync(consumer, context.CancellationToken);
        }
    }
}
