using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GettingStarted.Rabbitmq.Consumer2
{
    public class AnotherConsumer : IConsumer<RabbitMqContract>
    {
        private readonly ILogger<AnotherConsumer> logger;

        public AnotherConsumer(ILogger<AnotherConsumer> logger)
        {
            this.logger = logger;

        }
        public Task Consume(ConsumeContext<RabbitMqContract> context)
        {

            logger.LogError($"AnotherConsumer consumed message {context.Message.Text}");

            return Task.CompletedTask;

        }
    }

    public class AnotherConsumerDefinition: ConsumerDefinition<AnotherConsumer>
    {
        public AnotherConsumerDefinition()
        {
            base.EndpointName = "another-queue";
        }
    }


    public static class ConsumerInjector
    {
        public static IRegistrationConfigurator InjectConsumer2(this IRegistrationConfigurator services)
        {
            services.AddConsumer<AnotherConsumer, AnotherConsumerDefinition>();


            return services;
        }
    }
}
