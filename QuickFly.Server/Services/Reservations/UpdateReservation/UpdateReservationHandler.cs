using FluentValidation;
using QuickFly.Server.enums;
using QuickFly.Server.Models;
using QuickFly.Server.Services.Reservations.Exceptions.ReservationNotFoundException;
using QuickFly.Server.Shared.CQRS;
using QuickFly.Server.Shared.JsonHelper;
using QuickFly.Server.Shared.Shared.Shared.CQRS;

namespace QuickFly.Server.Services.Reservations.UpdateReservations;
public record UpdateReservationCommand
    (Guid Id,
    string Name,
    string LastName,
    string FligthNumber,
    DateTime DepartureDate,
    DateTime LandingDate,
    int TicketClass)
    : ICommand<UpdateReservationResult>;
public record UpdateReservationResult(bool IsSuccess);

public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("Reservation ID is required");


    }
}

internal class UpdateReservationCommandHandler : ICommandHandler<UpdateReservationCommand, UpdateReservationResult>
{
    private readonly IJsonFileHelper _jsonFileHelper;

    public UpdateReservationCommandHandler(IJsonFileHelper jsonFileHelper)
    {
        _jsonFileHelper = jsonFileHelper;
    }

    public async Task<UpdateReservationResult> Handle(UpdateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservations = await Task.Run(() => _jsonFileHelper.ReadFromJsonFile<Reservation>().ToList());
        var reservation = reservations.FirstOrDefault(p => p.Id == command.Id);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(command.Id);
        }

        reservation.Name = command.Name;
        reservation.LastName = command.LastName;
        reservation.FligthNumber = command.FligthNumber;
        reservation.DepartureDate = command.DepartureDate;
        reservation.LandingDate = command.LandingDate;
        reservation.TicketClass = (TicketClass)command.TicketClass;
        reservations.Remove(reservation);
        reservations.Add(reservation);
        await Task.Run(() => _jsonFileHelper.WriteToJsonFile(reservations));
        return new UpdateReservationResult(true);
    }
}

