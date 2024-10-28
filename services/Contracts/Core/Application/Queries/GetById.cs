using System;
using System.Threading;
using System.Threading.Tasks;
using Contracts.Core.Application.Abstractions.Queries;
using Contracts.Core.Domain.Enums;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Contracts.Core.Application.Queries;

public static class GetById
{
    public static class Requests
    {
        public sealed record Contract(Guid Id);
    }

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

    public sealed record Query(Requests.Contract Contract) : IAppQuery<Responses.Contract>;

    internal sealed class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query => query.Contract)
                .NotEmpty()
                .WithMessage("Η σύμβαση είναι υποχρεωτική");

            RuleFor(query => query.Contract.Id)
                .NotEmpty()
                .WithMessage("Το Id είναι υποχρεωτικό");
        }
    }

    internal sealed class QueryHandler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, Responses.Contract>
    {
        public async Task<Responses.Contract?> Handle(Query query, CancellationToken cancellationToken)
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
                where
                    Contracts.Id = @Id
            """;

            return await connection.QuerySingleOrDefaultAsync<Responses.Contract>(sql, new { query.Contract.Id });
        }
    }
}
