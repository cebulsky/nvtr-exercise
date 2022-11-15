using Jokes.Application;
using Jokes.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureAppConfiguration(builder =>
    {
        builder.AddJsonFile("appsettings.json");
    })
    .ConfigureServices((context, services ) =>
    {
        services.AddSqliteStorage(context.Configuration);
        services.AddTransient<JokesPuller>();
    })
    .Build();

host.Run();

