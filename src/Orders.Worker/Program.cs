using Serilog;
using MassTransit;
using System.Reflection;
using Orders.Core.Commands;
using Orders.Core.Infra.Facades;
using Orders.Worker.Configurations;
using Orders.Core.Infra.Repositories;

try
{
    var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        IConfigurationBuilder builder = new ConfigurationBuilder()
             .SetBasePath(hostContext.HostingEnvironment.ContentRootPath)
             .AddJsonFile("appsettings.json", true, true)
             .AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", true, true)
             .AddEnvironmentVariables();

        IConfiguration configuration = builder.Build();
        services.AddLogger();
        services.AddScoped<ICustomerFacade, CustomerFacade>();
        services.AddScoped<IAddressFacade, AddressFacade>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            config.AddConsumers(entryAssembly);

            config.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Send<OrderValidated>(s => s.UseSessionIdFormatter(c => c.Message.OrderID.ToString("D")));
                cfg.RequiresSession = true;
                cfg.Host(configuration["ServiceBus:ConnectionString"]);
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService(true);
    })
    .UseSerilog()
    .Build();

    await host.RunAsync();
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
