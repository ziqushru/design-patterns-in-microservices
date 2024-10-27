using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Application.Abstractions.Queries;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Providers.Core.Application.Queries;

public static class GetAll
{
    public static class Responses
    {
        public sealed record Provider
        {
            public required Guid Id { get; init; }
            public required string BrandName { get; init; }
            public required string Vat { get; init; }
            public required string Email { get; init; }
        }
    }

    public sealed record Query() : IAppQuery<IEnumerable<Responses.Provider>>;

    internal sealed class Handler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, IEnumerable<Responses.Provider>>
    {
        public async Task<IEnumerable<Responses.Provider>> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Providers.Id as Id,
                    Providers.BrandName as BrandName,
                    Providers.Vat as Vat,
                    Providers.Email as Email
                from
                    Providers
            """;

            return await connection.QueryAsync<Responses.Provider>(sql);
        }
    }
}
