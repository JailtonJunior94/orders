using Serilog;
using MassTransit;
using Orders.Core.Sagas;

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile("appsettings.json", true, true)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true)
        .AddEnvironmentVariables();

    builder.Services.AddMassTransit(config =>
    {
        config.AddSagaStateMachine<TravelStateMachine, TravelState>().InMemoryRepository();
        config.UsingAzureServiceBus((context, configurator) =>
        {
            configurator.Host("Endpoint=sb://pedidos-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iaiR8rJig+ev9cUV+06v18Ucrl8yFPwqzFX2/SyMID8=");
            configurator.ReceiveEndpoint("travel-saga", e =>
            {
                e.UseInMemoryOutbox();
                e.StateMachineSaga<TravelState>(context);
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
