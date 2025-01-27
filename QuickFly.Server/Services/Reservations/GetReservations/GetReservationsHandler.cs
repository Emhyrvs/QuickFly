using QuickFly.Server.Models;
using QuickFly.Server.Shared.CQRS;
using QuickFly.Server.Shared.JsonHelper;
using X.PagedList.Extensions;

namespace QuickFly.Server.Services.Reservations.GetReservations
{
    public record GetReservationsQuery(int? PageNumber = 1, int? PageSize = 10, string? Filter = "", string? Active = "", string? Direction = "") : IQuery<GetReservationsResult>;
    public record GetReservationsResult(IEnumerable<Reservation> Reservations, int TotalSize);

    public class GetReservationsQueryHandler
    : IQueryHandler<GetReservationsQuery, GetReservationsResult>
    {
        private readonly IJsonFileHelper _jsonFileHelper;

        public GetReservationsQueryHandler(IJsonFileHelper jsonFileHelper)
        {
            _jsonFileHelper = jsonFileHelper;
        }

        public async Task<GetReservationsResult> Handle(GetReservationsQuery query, CancellationToken cancellationToken)
        {

            var reservations = _jsonFileHelper.ReadFromJsonFile<Reservation>().AsQueryable();

            if (!string.IsNullOrEmpty(query.Filter))
            {
               reservations = reservations.Where(r =>
                    r.Name.Contains(query.Filter, StringComparison.OrdinalIgnoreCase) ||
                    r.LastName.Contains(query.Filter, StringComparison.OrdinalIgnoreCase) ||
                    r.FligthNumber.ToString().Contains(query.Filter) ||
                    r.TicketClass.ToString().Contains(query.Filter, StringComparison.OrdinalIgnoreCase));
            }
            var reservationList = reservations.AsEnumerable();



            bool ascending = string.IsNullOrEmpty(query.Direction) || query.Direction.Equals("asc", StringComparison.OrdinalIgnoreCase);
            Console.WriteLine(query.Active);
            reservationList = query.Active switch
            {
                "name" => ascending ? reservationList.OrderBy(r => r.Name).ToList() : reservationList.OrderByDescending(r => r.Name).ToList(),
                "lastName" => ascending ? reservationList.OrderBy(r => r.LastName).ToList() : reservationList.OrderByDescending(r => r.LastName).ToList(),
                "departureDate" => ascending ? reservationList.OrderBy(r => r.DepartureDate).ToList() : reservationList.OrderByDescending(r => r.DepartureDate).ToList(),
                "landingDate" => ascending ? reservationList.OrderBy(r => r.LandingDate).ToList() : reservationList.OrderByDescending(r => r.LandingDate).ToList(),
                "fligthNumber" => ascending ? reservationList.OrderBy(r => r.FligthNumber).ToList() : reservationList.OrderByDescending(r => r.FligthNumber).ToList(),
                "ticketClass" => ascending ? reservationList.OrderBy(r => r.TicketClass).ToList() : reservationList.OrderByDescending(r => r.TicketClass).ToList(),
                _ => reservationList
            };


            var pagedReservations = reservationList.ToPagedList(query.PageNumber ?? 1, query.PageSize ?? 10);

            int totalSize = pagedReservations.TotalItemCount;


            return new GetReservationsResult(pagedReservations, totalSize);
        }
    }

}
