using BuildingBlocks.Exceptions;

namespace QuickFly.Server.Services.Reservations.Exceptions.ReservationNotFoundException;
public class ReservationNotFoundException : NotFoundException
{
    public ReservationNotFoundException(Guid Id) : base("Reservation", Id)
    {
    }
}