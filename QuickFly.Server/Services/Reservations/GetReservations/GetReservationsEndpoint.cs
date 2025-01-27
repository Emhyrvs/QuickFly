using Carter;
using Mapster;
using MediatR;
using QuickFly.Server.Models;

namespace QuickFly.Server.Services.Reservations.GetReservations;

public record GetReservationsRequest(int? PageNumber = 1, int? PageSize = 10, string? Filter = "", string? Active = "", string? Direction = "");
public record GetReservationsResponse(List<Reservation> Reservations, int TotalSize);

public class GetReservationsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/reservations", async ([AsParameters] GetReservationsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetReservationsQuery>();

            var result = await sender.Send(query);

            var response = result.Adapt<GetReservationsResponse>();

            return Results.Ok(response);
        })
        .WithName("GetReservations")
        .Produces<GetReservationsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Reservations")
        .WithDescription("Get Reservations");
    }
}