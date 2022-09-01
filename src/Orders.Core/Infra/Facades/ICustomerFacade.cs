namespace Orders.Core.Infra.Facades;

public interface ICustomerFacade 
{
    Task<bool> CreateCustomerAsync(object customer);
}
