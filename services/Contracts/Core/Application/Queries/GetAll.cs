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
            public required Guid Id { get; init; }
            public required Guid ConsumerId { get; init; }
            public required Guid ProviderId { get; init; }
            public required ContractStatus Status { get; init; }
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

            using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Contracts.Id as Id,
                    Contracts.ConsumerId as ConsumerId,
                    Contracts.ProviderId as ProviderId,
                    Contracts.Status as Status
                from
                    Contracts
            """;

            return await connection.QueryAsync<Responses.Contract>(sql);
        }
    }
}
