using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Kafka.Consumer;

public static class Extension
{
    public static IServiceCollection AddConsumer<TMessage, THandler>(
        this IServiceCollection services,
        IConfiguration configuration) 
        where THandler : class, IMessageHandler<TMessage>
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka:OrderCreated"));

        services.AddHostedService<KafkaConsumer<TMessage>>();
        services.AddSingleton<IMessageHandler<TMessage>, THandler>();
        
        return services;
    }
}