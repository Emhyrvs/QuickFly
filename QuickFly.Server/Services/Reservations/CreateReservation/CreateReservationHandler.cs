using QuickFly.Server.enums;
using QuickFly.Server.Models;
using QuickFly.Server.Models.Dtos;
using QuickFly.Server.Shared.CQRS;
using QuickFly.Server.Shared.JsonHelper;
using QuickFly.Server.Shared.Shared.Shared.CQRS;
using System.Windows.Input;

namespace QuickFly.Server.Services.Reservations.CreateReservation;

public record CreateReservationCommand(
    string Name,
    string LastName,
    int FligthNumber,
    DateTime DepartureDate,
    DateTime LandingDate,
    int TicketClass
) : ICommand<CreateReservationResult>;
public record CreateReservationResult(Guid Id);
public class CreateReservationCommandHandler : ICommandHandler<CreateReservationCommand, CreateReservationResult>
{
    private readonly IJsonFileHelper _jsonFileHelper;

    public CreateReservationCommandHandler(IJsonFileHelper jsonFileHelper)
    {
        _jsonFileHelper = jsonFileHelper;
    }
    public async Task<CreateReservationResult> Handle(CreateReservationCommand command, CancellationToken cancellationToken)
    {
        var reservations = _jsonFileHelper.ReadFromJsonFile<Reservation>().ToList();

        

        var reservation = new Reservation
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            LastName = command.LastName,
            FligthNumber = command.FligthNumber,
            DepartureDate = command.DepartureDate,
            LandingDate = command.LandingDate,
            TicketClass = (TicketClass)command.TicketClass
        };

        if (reservations is null)
        {
            reservations = new List<Reservation>();
        }
        reservations.Add(reservation);
       _jsonFileHelper.WriteToJsonFile(reservations);

        return new CreateReservationResult(reservation.Id);
    }
}
