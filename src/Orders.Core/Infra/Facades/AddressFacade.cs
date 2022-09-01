namespace Orders.Core.Infra.Facades;

public class AddressFacade : IAddressFacade
{
    public Task<bool> ValidateAddress(string zipCode)
    {
        return Task.FromResult(true);
    }
}
