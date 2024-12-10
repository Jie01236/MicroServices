const amqplib = require('amqplib');

async function connectAndConsume() {
  const connection = await amqplib.connect('amqp://localhost');
  const channel = await connection.createChannel();

  await channel.assertExchange('payments_exchange', 'fanout', { durable: true });
  const { queue } = await channel.assertQueue('');

  await channel.bindQueue(queue, 'payments_exchange', '');

  channel.consume(queue, (msg) => {
    if (msg) {
      console.log('Événement consommé :', JSON.parse(msg.content.toString()));
      channel.ack(msg);
    }
  });
}

connectAndConsume();
