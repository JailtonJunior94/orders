using MassTransit;
using Orders.Core.Commands;
using MassTransit.Definition;
using MassTransit.ConsumeConfigurators;

namespace Orders.Worker.Consumers;

public class OrderConsumer : IConsumer<CreateOrder>
{
    private readonly ILogger<OrderConsumer> _logger;

    public OrderConsumer(ILogger<OrderConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CreateOrder> context)
    {
        _logger.LogInformation("Consuming message {@Message}", context.Message);
        
        return context.Publish<OrderCreated>(new
        {
            context.Message.CustomerID
        });
    }
}

public class OrderConsumerDefinition : ConsumerDefinition<OrderConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<OrderConsumer> consumerConfigurator)
    {

    }
}
