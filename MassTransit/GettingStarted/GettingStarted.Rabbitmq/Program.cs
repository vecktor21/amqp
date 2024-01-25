
using GettingStarted.Rabbitmq;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using GettingStarted.Rabbitmq.Consumer2;
using System.Reflection;
using System.Threading.Tasks;
using System;
using System.Threading;

var builder = CreateHost(args);

var tokenSource = new CancellationTokenSource();
var app = builder.Build();
var t = Task.Run(async () => await app.RunAsync(tokenSource.Token));

while (true)
{
    var key = Console.ReadKey();

    if(key.Key == ConsoleKey.Enter)
    {
        Console.WriteLine("BREAK");
        tokenSource.Cancel();
        break;
    }
}

await t;


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

            x.InjectConsumer2();

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