using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GettingStarted.Rabbitmq.Consumer
{
    public class RabbitmqConsumer : IConsumer<RabbitMqContract>
    {
        private readonly ILogger<RabbitmqConsumer> logger;

        public RabbitmqConsumer(ILogger<RabbitmqConsumer> logger)
        {
            this.logger = logger;
        }
        public Task Consume(ConsumeContext<RabbitMqContract> context)
        {
            logger.LogWarning($"First Consumer consumed message {context.Message.Text}");
            return Task.CompletedTask;
        }
    }
}
