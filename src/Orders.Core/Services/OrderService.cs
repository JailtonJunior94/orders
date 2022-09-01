using MassTransit;
using Orders.Core.Commands;
using Orders.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Orders.Core.Requests;

namespace Orders.Core.Services;

public class OrderService : IOrderService
{
    private readonly IPublishEndpoint _endpoint;
    private readonly ILogger<OrderService> _logger;
    private readonly string _prefix = $"{nameof(OrderService)}";

    public OrderService(IPublishEndpoint endpoint,
                        ILogger<OrderService> logger)
    {
        _logger = logger;
        _endpoint = endpoint;
    }

    public async Task CreateOrderAsync(Order order)
    {
        _logger.LogInformation($"[{_prefix}] [{nameof(CreateOrderAsync)}] [Pedido cadastrado e validado]");
        OrderValidated orderValidated = new OrderValidated(Guid.NewGuid());

        await _endpoint.Publish(orderValidated);
        await Task.CompletedTask;
    }
}