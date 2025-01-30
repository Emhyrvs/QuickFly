using QuickFly.Server.enums;

namespace QuickFly.Server.Models
{
    public class Reservation
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public String FligthNumber { get; set; } = default!;

        public DateTime DepartureDate { get; set; }

        public DateTime LandingDate { get; set; }


        public TicketClass TicketClass { get; set; }


    }
}
