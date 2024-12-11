using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace MessagingBus
{
    [Route("api/[controller]")]
    [ApiController]
    public class RabbitMQController : ControllerBase
    {
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest message)
        {
            var factory = new ConnectionFactory() { HostName = "localhost"};

            using var connection = await factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            string queueName = "demo-queue";
            await channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

            var body = Encoding.UTF8.GetBytes(message.Message);
            await channel.BasicPublishAsync(exchange: "", routingKey: queueName, body: body);

            Console.WriteLine("RabbiMQController : New message published");

            return Ok(Task.CompletedTask);
        }
    }
}
