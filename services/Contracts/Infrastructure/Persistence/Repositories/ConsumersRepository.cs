using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;

namespace Contracts.Infrastructure.Persistence.Repositories;

public class ConsumersRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Consumer>(applicationContext, mapperConfiguration), IConsumersRepository
{
}
