using AutoMapper;
using Contracts.Core.Application.Abstractions.Repositories;
using Contracts.Core.Domain.Entities;

namespace Contracts.Infrastructure.Persistence.Repositories;

public class ContractsRepository(
    ApplicationContext applicationContext,
    IConfigurationProvider mapperConfiguration)
    : BaseRepository<Contract>(applicationContext, mapperConfiguration), IContractsRepository
{
}
