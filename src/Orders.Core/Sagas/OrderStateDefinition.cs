using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using MassTransit.Definition;

namespace Orders.Core.Sagas;

public class OrderStateDefinition : SagaDefinition<OrderState>
{
    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
    {
        if (endpointConfigurator is IServiceBusEndpointConfigurator sbc)
        {
            sbc.RequiresSession = true;
            sbc.LockDuration = TimeSpan.FromMinutes(2);
            sbc.MaxAutoRenewDuration = TimeSpan.FromMinutes(5);
        }
        endpointConfigurator.UseInMemoryOutbox();
    }
}
