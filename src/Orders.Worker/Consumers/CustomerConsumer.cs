using MassTransit;
using Orders.Core.Commands;
using MassTransit.Definition;
using MassTransit.ConsumeConfigurators;

namespace Orders.Worker.Consumers;

public class CustomerConsumer : IConsumer<CreateCustomer>
{
    private readonly ILogger<CustomerConsumer> _logger;

    public CustomerConsumer(ILogger<CustomerConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<CreateCustomer> context)
    {
        /* Acessar Banco */

        /* Chamar outras APIs */

        /*  */
        
        _logger.LogInformation("Consuming message {@Message}", context.Message);

        return context.Publish<CustomerCreated>(new
        {
            context.Message.CustomerID
        });
    }
}

public class CustomerConsumerDefinition : ConsumerDefinition<CustomerConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<CustomerConsumer> consumerConfigurator)
    {

    }
}
