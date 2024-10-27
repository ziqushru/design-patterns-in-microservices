using AutoMapper;
using Providers.Core.Application.Abstractions.Repositories;
using Providers.Core.Domain.Entities;

namespace Providers.Infrastructure.Persistence.Repositories;

public class ProvidersRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Provider>(applicationContext, mapperConfiguration), IProvidersRepository
{
}
