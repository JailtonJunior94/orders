namespace Orders.Core.Commands;

public class OrderValidated
{
    public OrderValidated(Guid customerID)
    {
        CustomerID = customerID;
    }

    public Guid CustomerID { get; set; }
    public long OrderID { get; set; }
}
