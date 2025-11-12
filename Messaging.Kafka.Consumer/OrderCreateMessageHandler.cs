using Microsoft.Extensions.Logging;

namespace Messaging.Kafka.Consumer;

public class OrderCreateMessageHandler(ILogger<OrderCreateMessageHandler> logger)
    : IMessageHandler<OrderCreated>
{
    public Task HandleAsync(OrderCreated message, CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Order created: {MessageId} {MessageName}", message.Id, message.Name);
        
        return Task.CompletedTask;
    }
}