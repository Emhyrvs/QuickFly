using Newtonsoft.Json;
using QuickFly.Server.enums;
using QuickFly.Server.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace QuickFly.Server.Shared.JsonHelper
{
    public static class Initialize
    {
        private static readonly string JsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "reservations.json");

        public static void Run()
        {
            if (!File.Exists(JsonFilePath))
            {

                var reservations = new List<Reservation>
                {
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        Name = "John",
                        LastName = "Doe",
                        FligthNumber = "QF 1234",
                        DepartureDate = new DateTime(2025, 3, 15, 14, 30, 0),
                        LandingDate = new DateTime(2025, 3, 15, 18, 45, 0),
                        TicketClass = TicketClass.Business
                    },
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        Name = "Anna",
                        LastName = "Smith",
                        FligthNumber = "QF 4564",
                        DepartureDate = new DateTime(2025, 4, 20, 8, 0, 0),
                        LandingDate = new DateTime(2025, 4, 20, 12, 15, 0),
                        TicketClass = TicketClass.Economy
                    },
                    new Reservation
                    {
                        Id = Guid.NewGuid(),
                        Name = "Michael",
                        LastName = "Johnson",
                        FligthNumber = "QF 7849",
                        DepartureDate = new DateTime(2025, 5, 10, 22, 15, 0),
                        LandingDate = new DateTime(2025, 5, 11, 2, 30, 0),
                        TicketClass = TicketClass.FirstClass
                    }
                };

               
                string jsonString = JsonConvert.SerializeObject(reservations, Formatting.Indented);
                File.WriteAllText(JsonFilePath, jsonString);

                Console.WriteLine("Plik utworzony pomyślnie.");
            }
           
        }
    }
}
