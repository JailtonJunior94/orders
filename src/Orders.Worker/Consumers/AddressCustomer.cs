using MassTransit;
using MassTransit.Definition;
using Orders.Core.Infra.Facades;
using MassTransit.ConsumeConfigurators;
using Orders.Core.Commands;

namespace Orders.Worker.Consumers;

public class AddressConsumer : IConsumer<ValidateAddress>
{
    private readonly IAddressFacade _facade;
    private readonly ILogger<AddressConsumer> _logger;
    private readonly string _prefix = $"{nameof(AddressConsumer)}";

    public AddressConsumer(IAddressFacade facade,
                           ILogger<AddressConsumer> logger)
    {
        _logger = logger;
        _facade = facade;
    }

    public async Task Consume(ConsumeContext<ValidateAddress> context)
    {
        bool response = await _facade.ValidateAddress(context.MessageId.ToString());
        if (!response)
        {
            _logger.LogError($"[{_prefix}] [Validação do endereço]");
            return;
        }

        await context.Publish<AddressValidated>(new
        {
            context.Message.CustomerID
        });
    }
}

public class AddressConsumerDefinition : ConsumerDefinition<AddressConsumer>
{
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
                                              IConsumerConfigurator<AddressConsumer> consumerConfigurator)
    {

    }
}
