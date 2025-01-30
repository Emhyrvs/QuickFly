using FluentValidation;
using QuickFly.Server.Models;
using QuickFly.Server.Services.Reservations.Exceptions.ReservationNotFoundException;
using QuickFly.Server.Shared.CQRS;
using QuickFly.Server.Shared.JsonHelper;
using QuickFly.Server.Shared.Shared.Shared.CQRS;

namespace QuickFly.Server.Services.Reservations.DeleteReservation;
public record DeleteReservationCommand(Guid Id) : ICommand<DeleteReservationResult>;
public record DeleteReservationResult(bool IsSuccess);

public class DeleteReservationCommandValidator : AbstractValidator<DeleteReservationCommand>
{
    public DeleteReservationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Reservation ID is required");
    }
}
internal class DeleteReservationCommandHandler : ICommandHandler<DeleteReservationCommand, DeleteReservationResult>
{
    private readonly IJsonFileHelper _jsonFileHelper;

    public DeleteReservationCommandHandler(IJsonFileHelper jsonFileHelper)
    {
        _jsonFileHelper = jsonFileHelper;
    }

    public async Task<DeleteReservationResult> Handle(DeleteReservationCommand command, CancellationToken cancellationToken)
    {
        var reservations = await Task.Run(() => _jsonFileHelper.ReadFromJsonFile<Reservation>().ToList());
        var reservation = reservations.FirstOrDefault(p => p.Id == command.Id);

        if (reservation is null)
        {
            throw new ReservationNotFoundException(command.Id);
        }

        reservations.Remove(reservation);
        await Task.Run(() => _jsonFileHelper.WriteToJsonFile<Reservation>(reservations));

        return new DeleteReservationResult(true);
    }
}


