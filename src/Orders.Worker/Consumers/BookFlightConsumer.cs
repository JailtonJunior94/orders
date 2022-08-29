using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;
using Orders.Core.Commands;
using Orders.Core.Events;

namespace Orders.Worker.Consumers;

public class BookFlightConsumer : IConsumer<BookFlight>
{
    private readonly ILogger<BookFlightConsumer> _logger;

    public BookFlightConsumer(ILogger<BookFlightConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<BookFlight> context)
    {
        _logger.LogInformation("Consuming message {@Message}", context.Message);

        return context.Publish<FlightBooked>(new
        {
            context.Message.TravelId
        });
    }
}


public class BookFlightConsumerDefinition : ConsumerDefinition<BookFlightConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<BookFlightConsumer> consumerConfigurator)
    {
     
    }
}
