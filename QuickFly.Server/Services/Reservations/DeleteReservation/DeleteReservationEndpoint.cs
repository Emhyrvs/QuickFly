using Carter;
using Mapster;
using MediatR;

namespace QuickFly.Server.Services.Reservations.DeleteReservation;


   
    public record DeleteReservationResponse(bool IsSuccess);

    public class DeleteReservationEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/reservations/{id}", async (Guid id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteReservationCommand(id));

                var response = result.Adapt<DeleteReservationResponse>();

                return Results.Ok(response);
            })
            .WithName("DeleteReservation")
            .Produces<DeleteReservationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Delete Reservation")
            .WithDescription("Delete Reservation");
        }
    }

