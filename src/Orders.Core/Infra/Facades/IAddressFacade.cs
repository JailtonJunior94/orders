namespace Orders.Core.Infra.Facades;

public interface IAddressFacade
{
    Task<bool> ValidateAddress(string zipCode);
}
