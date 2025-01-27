﻿

namespace QuickFly.Server.Models.Dtos
{
    public record ReservationDto(
     string Name,
     string LastName,
     int FligthNumber,
     DateTime DepartureDate,
     DateTime LandingDate,
     int TicketClass);
}
