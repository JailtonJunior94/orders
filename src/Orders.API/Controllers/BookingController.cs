using MassTransit;
using Orders.Core.Events;
using Microsoft.AspNetCore.Mvc;

namespace Orders.API.Controllers;

[ApiController]
[Route("v1/bookings")]
public class BookingServiceController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    public BookingServiceController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    public async Task<IActionResult> Post(TravelBookingSubmitted trip)
    {
        var travelId = Guid.NewGuid();
        trip.TravelId = travelId;
        trip.FlightBooking.TravelId = travelId;
        trip.HotelBooking.TravelId = travelId;

        await _publishEndpoint.Publish(trip);
        return Ok(trip);
    }
}
