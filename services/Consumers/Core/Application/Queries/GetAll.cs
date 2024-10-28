using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Application.Abstractions.Queries;
using Consumers.Core.Domain.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Consumers.Core.Application.Queries;

public static class GetAll
{
    public static class Responses
    {
        public sealed record Consumer
        {
            public required Guid Id { get; init; }
            public required string FirstName { get; init; }
            public required string LastName { get; init; }
            public required string Email { get; init; }
            public required string CellPhone { get; init; }
            public required ConsumerType Type { get; init; }
        }
    }

    public sealed record Query() : IAppQuery<IEnumerable<Responses.Consumer>>;

    internal sealed class Handler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, IEnumerable<Responses.Consumer>>
    {
        public async Task<IEnumerable<Responses.Consumer>> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            await using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Consumers.Id as Id,
                    Consumers.FirstName as FirstName,
                    Consumers.LastName as LastName,
                    Consumers.Email as Email,
                    Consumers.CellPhone as CellPhone,
                    Consumers.Type as Type
                from
                    Consumers
            """;

            return await connection.QueryAsync<Responses.Consumer>(sql);
        }
    }
}
