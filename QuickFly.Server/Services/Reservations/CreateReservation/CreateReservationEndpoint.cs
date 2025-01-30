using Carter;
using Mapster;
using MediatR;
using QuickFly.Server.Shared.Shared.Shared.CQRS;

namespace QuickFly.Server.Services.Reservations.CreateReservation;


public record CreateReservationRequest(
    string Name,
    string LastName,
    string FligthNumber,
    DateTime DepartureDate,
    DateTime LandingDate,
    int TicketClass
) : ICommand<CreateReservationResult>;


public record CreateReservationResponse(Guid id);

public class CreateReservationEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/reservations",
              async (CreateReservationRequest request, ISender sender) =>
              {
                  var command = request.Adapt<CreateReservationCommand>();
                  var result = await sender.Send(command);
                  var response = result.Adapt<CreateReservationResponse>();

                  return Results.Created($"/reservation/{response.id}", response);
              })
                .WithName("CreateReservation")
                .Produces<CreateReservationResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Reservation")
                .WithDescription("Create Reservation");

    }
}
