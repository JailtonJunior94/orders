using Serilog;
using MassTransit;
using Orders.Core.Sagas;
using Orders.Core.Commands;
using Orders.Core.Services;
using Orders.Core.Interfaces;
using Orders.API.Configurations;
using Orders.Core.Infra.Repositories;
using Orders.Core.Infra.Facades;
using Orders.Core.Consumers;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    builder.Host.AddLogger();
    builder.Services.AddScoped<IOrderService, OrderService>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

    builder.Services.AddScoped<ICustomerFacade, CustomerFacade>();
    builder.Services.AddScoped<IAddressFacade, AddressFacade>();
    builder.Services.AddScoped<IOrderRepository, OrderRepository>();

    builder.Services.AddMassTransit(config =>
    {
        config.SetKebabCaseEndpointNameFormatter();
        config.AddConsumer<AddressConsumer, AddressConsumerDefinition>();
        config.AddConsumer<CustomerConsumer, CustomerConsumerDefinition>();
        config.AddConsumer<OrderConsumer, OrderConsumerDefinition>();
        config.AddSagaStateMachine<OrderStateMachine, OrderState, OrderStateDefinition>().MessageSessionRepository();
 
        config.UsingAzureServiceBus((context, configurator) =>
        {
            configurator.Host(builder.Configuration["ServiceBus:ConnectionString"]);
            configurator.Send<OrderValidated>(s => s.UseSessionIdFormatter(c => c.Message.CustomerID.ToString("D")));
            configurator.ConfigureEndpoints(context);
        });
    });

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception exception)
{
    Log.Fatal(exception, "Host encerrado inesperadamente");
}
finally
{
    Log.Information("Servidor desligando...");
    Log.CloseAndFlush();
}
