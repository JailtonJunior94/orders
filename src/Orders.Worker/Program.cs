using Serilog;
using Orders.Worker;
using MassTransit;
using System.Reflection;

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

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            config.AddConsumers(entryAssembly);

            config.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host("Endpoint=sb://pedidos-ns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=iaiR8rJig+ev9cUV+06v18Ucrl8yFPwqzFX2/SyMID8=");
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddMassTransitHostedService(true);
        services.AddHostedService<Worker>();
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
