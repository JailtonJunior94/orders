using Orders.Core.Requests;

namespace Orders.Core.Interfaces;

public interface IOrderService
{
    Task CreateOrderAsync(Order order);
}