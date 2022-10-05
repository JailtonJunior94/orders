using MassTransit;
using Microsoft.Extensions.Logging;
using Orders.Core.Commands;

namespace Orders.Core.Sagas;

public class OrderStateMachine : MassTransitStateMachine<OrderState>
{
    [Obsolete]
    public OrderStateMachine(ILogger<OrderStateMachine> logger)
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
                    CustomerID = context.CorrelationId,
                    OrderID = context.Instance.OrderID,
                }))
                .TransitionTo(AddressRequested)
        );

        /* Validação do endereço */
        During(AddressRequested,
            When(OrderCreated)
            .SendAsync(new Uri("queue:address"), context => context.Init<ValidateAddress>(new
            {
                CustomerID = context.CorrelationId,
            }))
            .TransitionTo(CustomerRequested)
        );

        /* Cadastro do cliente */
        During(CustomerRequested,
            When(AddressValidated)
                .SendAsync(new Uri("queue:customer"), context => context.Init<CreateCustomer>(new
                {
                    CustomerID = context.CorrelationId,
                }))
               .TransitionTo(PaymentRequested)
        );

        During(PaymentRequested,
            When(CustomerCreated)
            .Then(_ => Console.WriteLine("Cliente cadastrado"))
            .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    public State CustomerRequested { get; set; }
    public State PaymentRequested { get; set; }
    public State AddressRequested { get; set; }

    public Event<OrderValidated> OrderValidated { get; set; }

    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<CustomerCreated> CustomerCreated { get; set; }
    public Event<AddressValidated> AddressValidated { get; set; }
}
