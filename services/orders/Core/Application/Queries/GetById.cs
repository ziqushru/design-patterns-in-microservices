using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Abstractions.Queries;
using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Core.Domain.Enums;
using FluentValidation;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Core.Application.Queries;

public static class GetById
{
    public static class Requests
    {
        public sealed record Order(Guid Id);
    }

    public static class Responses
    {
        public sealed record Order
        {
            public required Guid Id { get; init; }
            public required Status Status { get; init; }
            public required int OrderNumber { get; init; }
            public required string StreetName { get; init; }
            public required string StreetNumber { get; init; }
            public required string ZipCode { get; init; }
            public required string Country { get; init; }
            public required string Town { get; init; }
            public required string LastName { get; init; }
            public required string FirstName { get; init; }
            public required string Email { get; init; }
            public required string CellPhone { get; init; }
            public required string PaymentMethodName { get; init; }
            public required string ShippingMethodName { get; init; }
            public required double Price { get; init; }
            public string? Note { get; init; }
            public string? TrackingNumber { get; init; }
        }
    }

    public sealed record Query(Requests.Order Order) : IAppQuery<Responses.Order>;

    internal sealed class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator()
        {
            RuleFor(query => query.Order)
                .NotEmpty()
                .WithMessage("Order is required");

            RuleFor(query => query.Order.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }

    internal sealed class QueryHandler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, Responses.Order>
    {
        public async Task<Responses.Order> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Orders.*
                from
                    Orders
                where
                    Orders.Id = @Id
                order by
                    Orders.DateSubmitted desc
            """;

            return await connection.QuerySingleOrDefaultAsync<Responses.Order>(sql, new { query.Order.Id });
        }
    }
}
