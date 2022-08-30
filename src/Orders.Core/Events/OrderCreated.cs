namespace Orders.Core.Commands;

public interface OrderCreated
{
    Guid CustomerID { get; }
}
