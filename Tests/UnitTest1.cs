using FluentAssertions;
using Moq;
using QuickFly.Server.Models;
using QuickFly.Server.Services.Reservations.GetReservations;
using QuickFly.Server.Shared.JsonHelper;

namespace QuickFly.Server.Tests.Services.Reservations
{
    public class GetReservationsQueryHandlerTests
    {
        private readonly Mock<IJsonFileHelper> _mockJsonFileHelper;
        private readonly GetReservationsQueryHandler _handler;


        public GetReservationsQueryHandlerTests()
        {
            _mockJsonFileHelper = new Mock<IJsonFileHelper>();
            _handler = new GetReservationsQueryHandler(_mockJsonFileHelper.Object);
        }

        [Fact]
        public async Task Handle_ShouldApplyFilter_WhenFilterIsProvided()
        {

            var query = new GetReservationsQuery(Filter: "John");

            var reservations = new List<Reservation>
            {
                new Reservation { Id = Guid.NewGuid(), Name = "Adam", LastName = "Zakrzewski", FligthNumber = 123, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(2) },
                new Reservation { Id = Guid.NewGuid(), Name = "Bartosz  ", LastName = "Zalewski", FligthNumber = 456, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(3) },
                new Reservation { Id = Guid.NewGuid(), Name = "Patryk", LastName = "Semeniuk", FligthNumber = 789, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(4) }
            };

            _mockJsonFileHelper.Setup(x => x.ReadFromJsonFile<Reservation>()).Returns(reservations);


            var result = await _handler.Handle(query, CancellationToken.None);

            result.Reservations.Should().HaveCount(2);
            result.Reservations.First().Name.Should().Be("John");
        }

        [Fact]
        public async Task Handle_ShouldSortReservationsByNameAscending_WhenActiveIsNameAndDirectionIsAsc()
        {

            var query = new GetReservationsQuery(Active: "name", Direction: "asc");

            var reservations = new List<Reservation>
            {
                new Reservation { Id = Guid.NewGuid(), Name = "Adam", LastName = "Zakrzewski", FligthNumber = 123, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(2) },
                new Reservation { Id = Guid.NewGuid(), Name = "Bartosz  ", LastName = "Zalewski", FligthNumber = 456, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(3) },
                new Reservation { Id = Guid.NewGuid(), Name = "Patryk", LastName = "Semeniuk", FligthNumber = 789, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(4) }
            };

            _mockJsonFileHelper.Setup(x => x.ReadFromJsonFile<Reservation>()).Returns(reservations);


            var result = await _handler.Handle(query, CancellationToken.None);


            result.Reservations.First().Name.Should().Be("Adam");
        }

        [Fact]
        public async Task Handle_ShouldSortReservationsByNameDescending_WhenActiveIsNameAndDirectionIsDesc()
        {

            var query = new GetReservationsQuery(Active: "name", Direction: "desc");

            var reservations = new List<Reservation>
            {
                new Reservation { Id = Guid.NewGuid(), Name = "Adam", LastName = "Zakrzewski", FligthNumber = 123, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(2) },
                new Reservation { Id = Guid.NewGuid(), Name = "Bartosz  ", LastName = "Zalewski", FligthNumber = 456, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(3) },
                new Reservation { Id = Guid.NewGuid(), Name = "Patryk", LastName = "Semeniuk", FligthNumber = 789, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(4) }
            };

            _mockJsonFileHelper.Setup(x => x.ReadFromJsonFile<Reservation>()).Returns(reservations);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Reservations.First().Name.Should().Be("Patryk");
        }

        [Fact]
        public async Task Handle_ShouldApplyPagination_WhenPageNumberAndPageSizeAreProvided()
        {
            var query = new GetReservationsQuery(PageNumber: 1, PageSize: 2);

            var reservations = new List<Reservation>
            {
                new Reservation { Id = Guid.NewGuid(), Name = "Adam", LastName = "Zakrzewski", FligthNumber = 123, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(2) },
                new Reservation { Id = Guid.NewGuid(), Name = "Bartosz  ", LastName = "Zalewski", FligthNumber = 456, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(3) },
                new Reservation { Id = Guid.NewGuid(), Name = "Patryk", LastName = "Semeniuk", FligthNumber = 789, TicketClass = enums.TicketClass.Economy, DepartureDate = DateTime.Now, LandingDate = DateTime.Now.AddHours(4) }
            };

            _mockJsonFileHelper.Setup(x => x.ReadFromJsonFile<Reservation>()).Returns(reservations);

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Reservations.Should().HaveCount(2);
            result.TotalSize.Should().Be(3);
        }
    }
}
