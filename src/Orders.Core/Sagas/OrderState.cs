using Automatonymous;

namespace Orders.Core.Sagas;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public long OrderID { get; set; }
    public long CustomerID { get; set; }
    public int Version { get; set; }
}
