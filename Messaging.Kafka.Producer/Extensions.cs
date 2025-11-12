using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Kafka.Producer;

public static class Extensions
{
    public static void AddProducer<TMessage>(
        this IServiceCollection services,
        IConfigurationSection section,
        IConfiguration configuration)
        where TMessage : class
    {
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka:Order"));
        services.AddSingleton<IKafkaProducer<TMessage>, KafkaProducer<TMessage>>();
    }
}