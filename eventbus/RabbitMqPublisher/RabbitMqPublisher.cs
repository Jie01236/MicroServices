using Microsoft.AspNetCore.Connections;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MessagingBus.RabbitMqPublisher
{
    public class RabbitMqPublisher<T> : IRabbitMqPublisher<T>
    {
        private readonly RabbitMqSettings _rabbitMqSettings;

        public RabbitMqPublisher(IOptions<RabbitMqSettings> rabbitMqSettings)
        {
            _rabbitMqSettings = rabbitMqSettings.Value;
        }



        public async Task PublishMessageAsync(T message, string queueName)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbitMqSettings.HostName,
                UserName = _rabbitMqSettings.UserName,
                Password = _rabbitMqSettings.Password
            };

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync(queueName, false, false, false, null);

            var messageJson = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(messageJson);

            var props = new BasicProperties();
            props.ContentType = "application/json"; // Indicates the content type
            props.DeliveryMode = DeliveryModes.Persistent;

            await Task.Run(() => channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body, basicProperties: props, mandatory: true));


        }
    }
}
