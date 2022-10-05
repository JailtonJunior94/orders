using MassTransit;
using Orders.Core.Commands;
using Orders.Core.Infra.Facades;
using Microsoft.Extensions.Logging;

namespace Orders.Core.Consumers;

public class CustomerConsumer : IConsumer<CreateCustomer>
{
    private readonly ICustomerFacade _facade;
    private readonly ILogger<CustomerConsumer> _logger;
    private readonly string _prefix = $"{nameof(CustomerConsumer)}";

    public CustomerConsumer(ICustomerFacade facade,
                            ILogger<CustomerConsumer> logger)
    {
        _logger = logger;
        _facade = facade;
    }

    public async Task Consume(ConsumeContext<CreateCustomer> context)
    {
        bool response = await _facade.CreateCustomerAsync(context.Message);
        if (!response)
        {
            _logger.LogError($"[{_prefix}] [Cadastrado de Cliente] [Erro ao cadastrar Cliente]");
        }

        _logger.LogInformation($"[{_prefix}] [Cadastrado de Cliente] [Cliente cadastrado com sucesso]");
        await context.Publish<CustomerCreated>(new
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
