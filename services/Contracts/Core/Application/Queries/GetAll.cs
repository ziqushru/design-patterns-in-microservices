using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Core.Application.Abstractions.Queries;
using Contracts.Core.Domain.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Contracts.Core.Application.Queries;

public static class GetAll
{
    public static class Responses
    {
        public sealed record Contract
        {
            public required Guid ContractId { get; init; }
            public required ContractStatus Status { get; init; }
            public required string ConsumerFullName { get; init; }
            public required ConsumerType ConsumerType { get; init; }
            public required string ProviderBrandName { get; init; }
        }
    }

    public sealed record Query() : IAppQuery<IEnumerable<Responses.Contract>>;

    internal sealed class Handler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, IEnumerable<Responses.Contract>>
    {
        public async Task<IEnumerable<Responses.Contract>> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            await using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Contracts.Id as ContractId,
                    Contracts.Status as ContractStatus,
                    Consumers.LastName + ' ' + Consumers.FirstName as ConsumerFullName,
                    Consumers.Type as ConsumerType,
                    Providers.BrandName as ProviderBrandName
                from
                    Contracts
                    inner join Consumers on Contracts.ConsumerId = Consumers.Id
                    inner join Providers on Contracts.ProviderId = Providers.Id
            """;

            return await connection.QueryAsync<Responses.Contract>(sql);
        }
    }
}
