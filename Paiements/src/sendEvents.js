const amqplib = require('amqplib');

let channel; 
  // Fonction pour envoyer un événement dans RabbitMQ
  export default async function sendPaymentEvent(paymentIntent) {
    const message = {
      paymentIntentId: paymentIntent.id,
      amount: paymentIntent.amount,
      currency: paymentIntent.currency,
      status: paymentIntent.status,
    };
  
    channel.publish(
      'payments_exchange',
      '',
      Buffer.from(JSON.stringify(message))
    );
  
    console.log('Événement envoyé dans RabbitMQ :', message);
  }