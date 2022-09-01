using MassTransit;
using Orders.Core.Commands;
using MassTransit.Definition;
using MassTransit.ConsumeConfigurators;
using Orders.Core.Infra.Repositories;

namespace Orders.Worker.Consumers;

public class OrderConsumer : IConsumer<CreateOrder>
{
    private readonly IOrderRepository _repository;
    private readonly ILogger<OrderConsumer> _logger;
    private readonly string _prefix = $"{nameof(OrderConsumer)}";

    public OrderConsumer(IOrderRepository repository,
                         ILogger<OrderConsumer> logger)
    {

        _logger = logger;
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CreateOrder> context)
    {
        bool response = await _repository.CreateOrder(context.Message);
        if (!response)
        {
            _logger.LogError($"[{_prefix}] [Cadastrado de pedido] [Erro ao cadastrar pedido]");
            return;
        }

        _logger.LogInformation($"[{_prefix}] [Cadastrado de pedido] [Pedido cadastrado com sucesso]");
        await context.Publish<OrderCreated>(new
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
