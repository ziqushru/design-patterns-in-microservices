using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Abstractions.Queries;
using Core.Application.Abstractions.Repositories;
using Core.Domain.Entities;
using Core.Domain.Enums;
using Dapper;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Core.Application.Queries;

public static class GetAll
{
    public static class Responses
    {
        public sealed record Order
        {
            public required Guid OrderId { get; init; }
            public required int OrderNumber { get; init; }
            public required DateTime DateSubmitted { get; init; }
            public required Status Status { get; init; }
            public required double OrderPrice { get; init; }
            public required string PaymentMethodName { get; init; }
            public required string ShippingMethodName { get; init; }
            public required string FullName { get; init; }
        }
    }

    public sealed record Query() : IAppQuery<IEnumerable<Responses.Order>>;

    internal sealed class Handler(
        IConfiguration configuration)
        : IAppQueryHandler<Query, IEnumerable<Responses.Order>>
    {
        public async Task<IEnumerable<Responses.Order>> Handle(Query query, CancellationToken cancellationToken)
        {
            var connectionString = configuration.GetConnectionString("App");

            using var connection = new MySqlConnection(connectionString);

            var sql = """
                select
                    Orders.Id as OrderId,
                    Orders.OrderNumber,
                    Orders.DateSubmitted,
                    Orders.Status,
                    Orders.Price as OrderPrice,
                    Orders.PaymentMethodName,
                    Orders.ShippingMethodName,
                    concat(Orders.FirstName, ' ', Orders.LastName) as FullName
                from
                    Orders
                order by
                    Orders.DateSubmitted desc
            """;

            return await connection.QueryAsync<Responses.Order>(sql);
        }
    }
}
