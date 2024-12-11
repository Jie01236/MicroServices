using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost", // Replace with your RabbitMQ server hostname or IP
    UserName = "guest",
    Password = "guest"
};

using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

string queueName = "demo-queue"; // Must match the queue name used in your endpoint

// Declare the queue (not necessary if already declared by the sender)
//channel.QueueDeclareAsync(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

Console.WriteLine("Waiting for messages...");

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (ch, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Received message: {message}");

    // copy or deserialise the payload
    // and process the message
    // ...
    await channel.BasicAckAsync(ea.DeliveryTag, false);
};
// this consumer tag identifies the subscription
// when it has to be cancelled
string consumerTag = await channel.BasicConsumeAsync(queueName, false, consumer);

Console.WriteLine("Press [enter] to exit.");
Console.ReadLine();

