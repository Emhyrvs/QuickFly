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
