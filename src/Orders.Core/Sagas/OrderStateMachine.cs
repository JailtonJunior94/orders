using MassTransit;
using Automatonymous;
using Orders.Core.Commands;

namespace Orders.Core.Sagas;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderValidated, context => context.CorrelateById(m => m.Message.CustomerID));
        Event(() => OrderCreated, context => context.CorrelateById(m => m.Message.CustomerID));
        Event(() => CustomerCreated, context => context.CorrelateById(m => m.Message.CustomerID));

        Initially(
            When(OrderValidated)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.CustomerID;
                })
                .SendAsync(new Uri("queue:create-order"), context => context.Init<CreateOrder>(new
                {
                    CustomerID = context.Instance.CorrelationId,
                }))
                .TransitionTo(CustomerRequested)
        );

        During(CustomerRequested,
            When(OrderCreated)
                .SendAsync(new Uri("queue:create-customer"), context => context.Init<CreateCustomer>(new
                {
                    CustomerID = context.Instance.CustomerID,
                }))
               .TransitionTo(PaymentRequested)
        );

        During(PaymentRequested,
            When(CustomerCreated)
            .Then(_ => Console.WriteLine("Cliente cadastrado"))
            .Finalize()
        );
    }

    public State CustomerRequested { get; set; }
    public State PaymentRequested { get; set; }

    public Event<OrderValidated> OrderValidated { get; set; }
    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<CustomerCreated> CustomerCreated { get; set; }
}
