namespace Orders.Core.Events;

public interface FlightBooked
{
    Guid TravelId { get; set; }
}
