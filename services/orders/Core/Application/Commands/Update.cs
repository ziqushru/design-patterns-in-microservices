using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Core.Application.Abstractions.Commands;
using Core.Application.Abstractions.Repositories;
using Core.Application.Abstractions.Services;
using Core.Domain.Abstractions.Exceptions;
using Core.Domain.Entities;
using Core.Domain.Enums;
using FluentValidation;

namespace Core.Application.Commands;

public static class Update
{
    public static class Requests
    {
        public sealed record Order(
            Guid Id,
            Status Status,
            string FirstName,
            string LastName,
            string Email,
            string CellPhone,
            string StreetName,
            string StreetNumber,
            string ZipCode,
            string Country,
            string Town,
            string ShippingMethodName,
            string PaymentMethodName,
            string? Note,
            string? TrackingNumber);
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
            RuleFor(command => command.Order)
                .NotEmpty()
                .WithMessage("Η παραγγελία είναι υποχρεωτική");

            RuleFor(command => command.Order.Status)
                .IsInEnum()
                .WithMessage("Ο καθορισμός της κατάστασης της παραγγελίας είναι υποχρεωτικός");

            RuleFor(command => command.Order.FirstName)
                .NotEmpty()
                .WithMessage("Το όνομα είναι υποχρεωτικό");

            RuleFor(command => command.Order.LastName)
                .NotEmpty()
                .WithMessage("Το επώνυμο είναι υποχρεωτικό");

            RuleFor(command => command.Order.Email)
                .EmailAddress()
                .WithMessage("Το email είναι υποχρεωτικό");

            RuleFor(command => command.Order.CellPhone)
                .Matches("^[0-9]*$")
                .WithMessage("Το κινητό τηλέφωνο είναι υποχρεωτικό");

            RuleFor(command => command.Order.StreetName)
                .NotEmpty()
                .WithMessage("H οδός είναι υποχρεωτική");

            RuleFor(command => command.Order.StreetNumber)
                .NotEmpty()
                .WithMessage("Ο αριθμός είναι υποχρεωτικός");

            RuleFor(command => command.Order.ZipCode)
                .NotEmpty()
                .WithMessage("Ο ταχυδρομικός κώδικας είναι υποχρεωτικός");

            RuleFor(command => command.Order.Town)
                .NotEmpty()
                .WithMessage("Η πόλη είναι υποχρεωτική");

            RuleFor(command => command.Order.Country)
                .NotEmpty()
                .WithMessage("Η χώρα είναι υποχρεωτική");

            RuleFor(command => command.Order.ShippingMethodName)
                .NotEmpty()
                .WithMessage("Ο τρόπος αποστολής είναι υποχρεωτικός");

            RuleFor(command => command.Order.PaymentMethodName)
                .NotEmpty()
                .WithMessage("Ο τρόπος πληρωμής είναι υποχρεωτικός");
        }
    }

    internal sealed class CommandHandler(
        IMapper mapper,
        IOrdersRepository ordersRepository,
        IOrdersService ordersService)
        : IAppCommandHandler<Command, Responses.Order>
    {
        public async Task<Responses.Order> Handle(Command command, CancellationToken cancellationToken)
        {
            var order = await ordersRepository.GetByIdAsync(command.Order.Id, cancellationToken)
                ?? throw new NotFoundException("Δεν βρέθηκε η παραγγελία");

            mapper.Map(command.Order, order);

            await ordersService.UpdateAsync(order, cancellationToken);

            return new Responses.Order(command.Order.Id);
        }
    }
}
