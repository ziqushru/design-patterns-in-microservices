using System;
using System.Threading;
using System.Threading.Tasks;
using Providers.Core.Application.Abstractions.Queries;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Providers.Core.Application.Queries;

public static class GetById
{
    public static class Requests
    {
        public sealed record Provider(Guid Id);
    }

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

    public sealed record Query(Requests.Provider Provider) : IAppQuery<Responses.Provider>;

    internal sealed class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query => query.Provider)
                .NotEmpty()
                .WithMessage("Ο πάροχος είναι υποχρεωτικός");

            RuleFor(query => query.Provider.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class QueryHandler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, Responses.Provider>
    {
        public async Task<Responses.Provider?> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            await using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Providers.Id as Id,
                    Providers.BrandName as BrandName,
                    Providers.Vat as Vat,
                    Providers.Email as Email
                from
                    Providers
                where
                    Providers.Id = @Id
            """;

            return await connection.QuerySingleOrDefaultAsync<Responses.Provider>(sql, new { query.Provider.Id });
        }
    }
}
