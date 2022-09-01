namespace Orders.Core.Commands;

public class ValidateAddress
{
    public Guid CustomerID { get; set; }
    public string Zipcode { get; set; }
}