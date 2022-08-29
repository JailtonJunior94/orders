using Automatonymous;

namespace Orders.Core.Sagas;

public class TravelState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int HotelId { get; set; }
    public int Version { get; set; }
}
