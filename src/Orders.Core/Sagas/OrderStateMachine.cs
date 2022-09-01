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
        Event(() => AddressValidated, context => context.CorrelateById(m => m.Message.CustomerID));

        Initially(
            When(OrderValidated)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.CustomerID;
                    context.Instance.OrderID = context.Data.OrderID;
                })
                .SendAsync(new Uri("queue:order"), context => context.Init<CreateOrder>(new
                {
                    CustomerID = context.Instance.CorrelationId,
                    OrderID = context.Instance.OrderID,
                }))
                .TransitionTo(AddressRequested)
        );

        /* Validação do endereço */
        During(AddressRequested,
            When(OrderCreated)
            .SendAsync(new Uri("queue:address"), context => context.Init<ValidateAddress>(new
            {
                CustomerID = context.Instance.CorrelationId,
            }))
            .TransitionTo(CustomerRequested)
        );

        /* Cadastro do cliente */
        During(CustomerRequested,
            When(AddressValidated)
                .SendAsync(new Uri("queue:customer"), context => context.Init<CreateCustomer>(new
                {
                    CustomerID = context.Instance.CorrelationId,
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
    public State AddressRequested { get; set; }

    public Event<OrderValidated> OrderValidated { get; set; }
    
    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<CustomerCreated> CustomerCreated { get; set; }
    public Event<AddressValidated> AddressValidated { get; set; }
}
