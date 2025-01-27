using Carter;
using Mapster;
using MediatR;
using QuickFly.Server.Models;

namespace QuickFly.Server.Services.Reservations.GetReservationById;

public record GetReservationByIdResponse(Reservation Reservation);

public class GetReservationByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/reservation/{id}", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new GetReservationByIdQuery(id));

            var response = result.Adapt<GetReservationByIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetReservation ById")
        .Produces<GetReservationByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get ReservationBy Id")
        .WithDescription("Get ReservationBy Id");
    }
}

