namespace Orders.Core.Infra.Repositories;

public interface IOrderRepository
{
    Task<bool> CreateOrder(object order);
}