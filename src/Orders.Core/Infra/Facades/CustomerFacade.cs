namespace Orders.Core.Infra.Facades;

public class CustomerFacade : ICustomerFacade
{
    public Task<bool> CreateCustomerAsync(object customer)
    {
        return Task.FromResult(true); ;
    }
}
