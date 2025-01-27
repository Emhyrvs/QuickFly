using QuickFly.Server.Models;
using QuickFly.Server.Services.Reservations.Exceptions.ReservationNotFoundException;
using QuickFly.Server.Shared.CQRS;
using QuickFly.Server.Shared.JsonHelper;

namespace QuickFly.Server.Services.Reservations.GetReservationById;

public record GetReservationByIdQuery(Guid Id) : IQuery<GetReservationByIdResult>;
public record GetReservationByIdResult(Reservation Reservation);

internal class GetReservationByIdQueryHandler
    : IQueryHandler<GetReservationByIdQuery, GetReservationByIdResult>
{
    private readonly IJsonFileHelper _jsonFileHelper;

    public GetReservationByIdQueryHandler(IJsonFileHelper jsonFileHelper)
    {
        _jsonFileHelper = jsonFileHelper;
    }
    public async Task<GetReservationByIdResult> Handle(GetReservationByIdQuery query, CancellationToken cancellationToken)
    {
        var reservations = _jsonFileHelper.ReadFromJsonFile<Reservation>();


        var reservation = reservations.FirstOrDefault(a => a.Id == query.Id);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(query.Id);
        }

        return new GetReservationByIdResult(reservation);

    }
}