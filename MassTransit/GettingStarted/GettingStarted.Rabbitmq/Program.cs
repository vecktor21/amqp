
using GettingStarted.Rabbitmq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

var builder = CreateHost(args);


var app = builder.Build();
await app.RunAsync();


static IHostBuilder CreateHost(string[] args)
{
    var builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices((context, services) =>
    {
        services.AddLogging(x=> x.AddConsole());

        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            var assembly = Assembly.GetExecutingAssembly();

            var s = assembly.GetTypes();

            x.AddConsumers(assembly);

            x.UsingRabbitMq((rabbitMqContext, rabbitMqConfig) =>
            {
                rabbitMqConfig.Host("localhost", "/", hostConfig =>
                {
                    hostConfig.Username("guest");
                    hostConfig.Password("guest");
                });

                rabbitMqConfig.ConfigureEndpoints(rabbitMqContext);
            });
        });


        services.AddHostedService<Worker>();
    });


    return builder;
}