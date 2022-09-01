namespace Orders.Core.Infra.Repositories;

public class OrderRepository : IOrderRepository
{
    public Task<bool> CreateOrder(object order)
    {
        return Task.FromResult(true);
    }
}