using Confluent.Kafka;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Messaging.Kafka.Consumer;

public class KafkaConsumer<TMessage> : BackgroundService
{
    private readonly IMessageHandler<TMessage> _handler;
    private readonly string _topic;
    private readonly IConsumer<string, TMessage> _consumer;

    public KafkaConsumer(IOptions<KafkaSettings> kafkaSettings, IMessageHandler<TMessage> handler)
    {
        _handler = handler;
        var config = new ConsumerConfig
        {
            BootstrapServers = kafkaSettings.Value.BootstrapServers,
            GroupId = kafkaSettings.Value.GroupId,
        };
        
        _topic = kafkaSettings.Value.Topic;

        _consumer = new ConsumerBuilder<string, TMessage>(config)
            .SetValueDeserializer(new KafkaValueDeserializer<TMessage>())
            .Build();
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() => ConsumeAsync(stoppingToken), stoppingToken);
    }

    private async Task? ConsumeAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(_topic);
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);
                
                await _handler.HandleAsync(result.Message.Value, stoppingToken).ConfigureAwait(false);
            }
        }
        catch (Exception ex)
        {
            //
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _consumer.Close();
        return base.StopAsync(stoppingToken);
    }
}