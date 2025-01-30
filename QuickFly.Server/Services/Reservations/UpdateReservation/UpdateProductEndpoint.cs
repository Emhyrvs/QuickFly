using Carter;
using Mapster;
using MediatR;
using QuickFly.Server.Services.Reservations.UpdateReservations;

namespace QuickFly.Server.Services.Reservations.UpdateReservation;
public record UpdateReservationRequest(
    Guid Id,
    string Name,
    string LastName,
    string FligthNumber,
    DateTime DepartureDate,
    DateTime LandingDate,
    int TicketClass);
public record UpdateReservationResponse(bool IsSuccess);

public class UpdateReservationEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/reservations",
            async (UpdateReservationRequest request, ISender sender) =>
            {
                var command = request.Adapt<UpdateReservationCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<UpdateReservationResponse>();

                return Results.Ok(response);
            })
            .WithName("UpdateReservation")
            .Produces<UpdateReservationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Reservation")
            .WithDescription("Update Reservation");
    }
}

