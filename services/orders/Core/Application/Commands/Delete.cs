using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Abstractions.Commands;
using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Entities;
using Core.Domain.Enums;
using FluentValidation;

namespace Core.Application.Commands;

public static class Delete
{
    public static class Requests
    {
        public sealed record Order(Guid Id);
    }

    public static class Responses
    {
        public sealed record Order(Guid Id);
    }

    public sealed record Command(Requests.Order Order) : IAppCommand<Responses.Order>;

    internal sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Requests.Order, Order>();
        }
    }

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(command => command.Order.Id)
                .NotEmpty()
                .WithMessage("Id is required");
        }
    }

    internal sealed class CommandHandler(
        IOrdersService ordersService,
        IOrdersRepository ordersRepository)
        : IAppCommandHandler<Command, Responses.Order>
    {
        public async Task<Responses.Order> Handle(Command command, CancellationToken cancellationToken)
        {
            var order = await ordersRepository.GetByIdAsync(command.Order.Id, cancellationToken)
                ?? throw new Exception("Δεν βρέθηκε η παραγγελία");

            await ordersService.UpdateAsync(order, cancellationToken);

            await ordersService.DeleteAsync(command.Order.Id, cancellationToken);

            return new Responses.Order(command.Order.Id);
        }
    }
}
