using Serilog;
using MassTransit;
using Orders.Worker;
using System.Reflection;
using Orders.Worker.Configurations;

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

        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            Assembly entryAssembly = Assembly.GetEntryAssembly();
            config.AddConsumers(entryAssembly);

            config.UsingAzureServiceBus((context, cfg) =>
            {
                cfg.Host(configuration["ServiceBus:ConnectionString"]);
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
