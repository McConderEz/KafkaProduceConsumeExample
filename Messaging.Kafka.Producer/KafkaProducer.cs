using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace Messaging.Kafka.Producer;

public class KafkaProducer<TMessage> : IKafkaProducer<TMessage>
{
    private readonly IProducer<string, TMessage> _producer;
    private readonly string _topic;

    public KafkaProducer(IOptions<KafkaSettings> kafkaSettings)
    {
        _ = kafkaSettings ?? throw new ArgumentNullException(nameof(kafkaSettings));
        
        var config = new ProducerConfig()
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
        };

        _producer = new ProducerBuilder<string, TMessage>(config)
            .SetValueSerializer(new KafkaJsonSerializer<TMessage>())
            .Build();
        _topic = kafkaSettings.Value.Topic;
    }
    
    public async Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default)
    {
        await _producer.ProduceAsync(_topic, new Message<string, TMessage>()
            {
                Key = "uniq1",
                Value = message
            }, cancellationToken)
            .ConfigureAwait(false);
    }

    public void Dispose()
    {
        _producer?.Dispose();
    }
}