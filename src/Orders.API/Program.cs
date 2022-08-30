using Serilog;
using MassTransit;
using Orders.Core.Sagas;
using Orders.Core.Services;
using Orders.Core.Interfaces;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    // builder.Host.AddLogger();
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddMassTransit(config =>
    {
        config.AddSagaStateMachine<OrderStateMachine, OrderState>().InMemoryRepository();
        config.UsingAzureServiceBus((context, configurator) =>
        {
            configurator.Host("Endpoint=sb://orders-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=heJiq7Tpe/L50cXGNI7v5oH0+iGLKxqtDNMbNMSEzaw=");
            configurator.ReceiveEndpoint("order-saga", e =>
            {
                e.UseInMemoryOutbox();
                e.StateMachineSaga<OrderState>(context);
            });
        });
    });

    builder.Services.AddMassTransitHostedService();
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
