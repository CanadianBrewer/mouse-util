using mouse_util;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("log.txt", rollingInterval: RollingInterval.Day, rollOnFileSizeLimit: true)
    .CreateLogger();

Log.Logger.Information("starting mouse-util");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services => {
        services.AddHostedService<Worker>();
        services.AddLogging(builder => builder.AddSerilog(Log.Logger));
    })
    .Build();

await host.RunAsync();