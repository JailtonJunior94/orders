namespace Orders.Core.Commands;

public class CreateOrder
{
    public Guid CustomerID { get; set; }
    public long OrderID { get; set; }
}