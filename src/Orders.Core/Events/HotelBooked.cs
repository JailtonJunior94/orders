namespace Orders.Core.Events;

public interface HotelBooked
{
    int HotelId { get; }
    Guid TravelId { get; set; }
}
