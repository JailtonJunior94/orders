using Automatonymous;
using MassTransit;
using Orders.Core.Commands;
using Orders.Core.Events;

namespace Orders.Core.Sagas;

public class TravelStateMachine : MassTransitStateMachine<TravelState>
{
    public TravelStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => TravelBookingSubmitted, context => context.CorrelateById(m => m.Message.TravelId));
        Event(() => FlightBooked, context => context.CorrelateById(m => m.Message.TravelId));
        Event(() => HotelBooked, context => context.CorrelateById(m => m.Message.TravelId));

        Initially(
            When(TravelBookingSubmitted)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.TravelId;
                    context.Instance.HotelId = context.Data.HotelBooking.HotelId;
                })
                .SendAsync(new Uri("queue:book-flight"), context => context.Init<BookFlight>(new
                {
                    TravelId = context.Instance.CorrelationId,
                    context.Data.FlightBooking.From,
                    context.Data.FlightBooking.To,
                    context.Data.FlightBooking.Departure
                }))
                .TransitionTo(FlightBookingRequested)
        );

        During(FlightBookingRequested,
            When(FlightBooked)
                .SendAsync(new Uri("queue:book-hotel"), context => context.Init<BookHotel>(new
                {
                    context.Instance.HotelId,
                    context.Data.TravelId
                }))
                .TransitionTo(HotelBookingRequested)
        );

        During(HotelBookingRequested,
            When(HotelBooked)
                .Then(_ => Console.WriteLine("Hotel reservado"))
                .Finalize()
            );
    }

    public State HotelBookingRequested { get; set; }
    public State FlightBookingRequested { get; set; }

    public Event<TravelBookingSubmitted> TravelBookingSubmitted { get; set; }
    public Event<FlightBooked> FlightBooked { get; set; }
    public Event<HotelBooked> HotelBooked { get; set; }
}
