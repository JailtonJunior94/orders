using Orders.Core.Commands;

namespace Orders.Core.Events;

public class TravelBookingSubmitted
{
    public Guid TravelId { get; set; }
    public BookFlight FlightBooking { get; set; }
    public BookHotel HotelBooking { get; set; }
}
