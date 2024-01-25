using MassTransit;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GettingStarted
{
    public class Worker : BackgroundService
    {
        private readonly IBus bus;

        public Worker(IBus bus)
        {
            this.bus = bus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                await bus.Publish(new Contracts.GettingStarted
                {
                    Value = $"The time is {DateTimeOffset.Now}"
                }, stoppingToken);


                await Task.Delay(1000);
            }
        }
    }
}