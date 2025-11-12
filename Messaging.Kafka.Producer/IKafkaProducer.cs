namespace Messaging.Kafka.Producer;

/// <summary>
/// Издатель сообщений в Кафка.
/// </summary>
/// <typeparam name="TMessage">Тип сообщения.</typeparam>
public interface IKafkaProducer<in TMessage> : IDisposable
{
    /// <summary>
    /// Издать сообщение.
    /// </summary>
    /// <param name="message">Сообщение.</param>
    /// <param name="cancellationToken">Токен отмены операции.</param>
    /// <returns></returns>
    Task ProduceAsync(TMessage message, CancellationToken cancellationToken = default);
}