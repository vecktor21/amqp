using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GettingStarted.Rabbitmq
{
    public class Worker : BackgroundService
    {
        private readonly IBus bus;
        private readonly ILogger<Worker> logger;
        private readonly ISendEndpointProvider sendEnpointProvider;

        public Worker(IBus bus, ILogger<Worker> logger, ISendEndpointProvider sendEnpointProvider)
        {
            this.bus = bus;
            this.logger = logger;
            this.sendEnpointProvider = sendEnpointProvider;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var text = $"Text: {DateTime.Now}";
                /*await bus.Publish(new RabbitMqContract
                {
                    Text = text
                });*/

                var endpoint = await sendEnpointProvider.GetSendEndpoint(new Uri("queue:another-queue"));

                await endpoint.Send(new RabbitMqContract
                {
                    Text = text
                }, stoppingToken);

                logger.LogCritical($"Worker sended: {text}");

                await Task.Delay(1000);
            }
        }
    }
}
