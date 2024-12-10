namespace MessagingBus.RabbitMqPublisher
{
    public interface IRabbitMqPublisher <T>
    {
        Task PublishMessageAsync(T message, string queueName);
    }
}
