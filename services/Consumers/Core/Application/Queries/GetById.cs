using System;
using System.Threading;
using System.Threading.Tasks;
using Consumers.Core.Application.Abstractions.Queries;
using Consumers.Core.Domain.Enums;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Consumers.Core.Application.Queries;

public static class GetById
{
    public static class Requests
    {
        public sealed record Consumer(Guid Id);
    }

    public static class Responses
    {
        public sealed record Consumer
        {
            public required Guid Id { get; init; }
            public required string LastName { get; init; }
            public required string FirstName { get; init; }
            public required string Email { get; init; }
            public required string CellPhone { get; init; }
            public required ConsumerType Type { get; init; }
        }
    }

    public sealed record Query(Requests.Consumer Consumer) : IAppQuery<Responses.Consumer>;

    internal sealed class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query => query.Consumer)
                .NotEmpty()
                .WithMessage("Ο καταναλωτής είναι υποχρεωτικός");

            RuleFor(query => query.Consumer.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class QueryHandler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, Responses.Consumer>
    {
        public async Task<Responses.Consumer?> Handle(Query query, CancellationToken cancellationToken)
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
                where
                    Consumers.Id = @Id
            """;

            return await connection.QuerySingleOrDefaultAsync<Responses.Consumer>(sql, new { query.Consumer.Id });
        }
    }
}
