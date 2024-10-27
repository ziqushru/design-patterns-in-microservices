using AutoMapper;
using Consumers.Core.Application.Abstractions.Repositories;
using Consumers.Core.Domain.Entities;

namespace Consumers.Infrastructure.Persistence.Repositories;

public class ConsumersRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Consumer>(applicationContext, mapperConfiguration), IConsumersRepository
{
}
