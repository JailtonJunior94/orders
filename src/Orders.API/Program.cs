using Serilog;
using MassTransit;
using Orders.Core.Sagas;
using Orders.Core.Commands;
using Orders.Core.Services;
using Orders.Core.Interfaces;
using Orders.API.Configurations;
using Orders.Core.Infra.Repositories;

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

    builder.Services.AddMassTransit(config =>
    {
        config.AddSagaStateMachine<OrderStateMachine, OrderState>().MessageSessionRepository();
        config.UsingAzureServiceBus((context, configurator) =>
        {
            configurator.Host(builder.Configuration["ServiceBus:ConnectionString"]);
            configurator.Send<OrderValidated>(s => s.UseSessionIdFormatter(c => c.Message.OrderID.ToString("D")));
            configurator.ReceiveEndpoint("order-saga", e =>
            {
                e.RequiresSession = true;
                e.ConfigureSaga<OrderState>(context);
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
