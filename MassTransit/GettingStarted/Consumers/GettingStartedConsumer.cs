using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

public class GettingStartedConsumer : IConsumer<Contracts.GettingStarted>
{
    private readonly ILogger<GettingStartedConsumer> logger;

    public GettingStartedConsumer(ILogger<GettingStartedConsumer> logger)
    {
        this.logger = logger;
    }
    public Task Consume(ConsumeContext<Contracts.GettingStarted> context)
    {
        logger.LogInformation($"GettingStarted consumed; Text: {context.Message.Value}");

        return Task.CompletedTask;
    }
}