using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;

namespace Contracts.Infrastructure.Persistence.Repositories;

public class ProvidersRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Provider>(applicationContext, mapperConfiguration), IProvidersRepository
{
}
