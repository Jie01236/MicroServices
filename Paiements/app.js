require('dotenv').config();
const fastify = require('fastify')({ logger: true });
const stripe = require('stripe')(process.env.STRIPE_SECRET_KEY);
const sqlite3 = require('sqlite3').verbose();
const db = new sqlite3.Database('./payments.db'); // Chemin vers le fichier SQLite

// Middleware pour analyser les requêtes JSON
fastify.register(require('@fastify/formbody'));

// Créer la table SQLite si elle n'existe pas
db.serialize(() => {
  db.run(`
    CREATE TABLE IF NOT EXISTS payments (
      id INTEGER PRIMARY KEY AUTOINCREMENT,
      payment_intent_id TEXT NOT NULL,
      amount INTEGER NOT NULL,
      currency TEXT NOT NULL,
      status TEXT NOT NULL,
      created_at DATETIME DEFAULT CURRENT_TIMESTAMP
    )
  `);
});

// Fonction pour sauvegarder un paiement dans SQLite après le succès
async function savePaymentToDB(paymentIntent) {
  return new Promise((resolve, reject) => {
    const stmt = db.prepare(`
      INSERT INTO payments (payment_intent_id, amount, currency, status) 
      VALUES (?, ?, ?, ?)
    `);

    stmt.run(
      paymentIntent.id,
      paymentIntent.amount,
      paymentIntent.currency,
      paymentIntent.status,
      function (err) {
        if (err) reject(err);
        else resolve();
      }
    );

    stmt.finalize();
  });
}

// Route principale
fastify.get('/', async (request, reply) => {
    reply.send({ message: 'API de Paiements - Projet d\'école' });
});

// Route pour créer un paiement
fastify.post('/api/payment', async (request, reply) => {
  const { amount, currency, paymentMethodId } = request.body;

  if (!amount || !currency || !paymentMethodId) {
    return reply.status(400).send({ error: 'Veuillez fournir amount, currency, et paymentMethodId' });
  }

  try {
    const paymentIntent = await stripe.paymentIntents.create({
      amount,
      currency,
      payment_method: paymentMethodId,
      confirm: true,
      automatic_payment_methods: {
        enabled: true,
        allow_redirects: 'never',
      },
    });

    // Sauvegarder dans la base SQLite seulement si le paiement est réussi
    if (paymentIntent.status === 'succeeded') {
      await savePaymentToDB(paymentIntent);
    }

    reply.send({
      success: true,
      paymentIntentId: paymentIntent.id,
      status: paymentIntent.status,
    });
  } catch (error) {
    fastify.log.error(error);
    reply.status(500).send({ error: error.message });
  }
});

fastify.get('/api/payments', async (request, reply) => {
    return new Promise((resolve, reject) => {
      db.all('SELECT * FROM payments WHERE status = "succeeded"', (err, rows) => {
        if (err) {
          fastify.log.error('Erreur lors de la récupération des paiements :', err);
          reply.status(500).send({ error: 'Erreur lors de la récupération des paiements' });
          return reject(err);
        }
  
        fastify.log.info('Données récupérées depuis la base SQLite :', rows);
  
        // Envoyer la réponse uniquement une seule fois
        reply.send({
          success: true,
          data: rows,
        });
        resolve();
      });
    });
  });
  

  
fastify.get('/api/payment-status/:paymentIntentId', async (request, reply) => {
    const { paymentIntentId } = request.params;
  
    try {
      const paymentIntent = await stripe.paymentIntents.retrieve(paymentIntentId);
  
      reply.send({
        status: paymentIntent.status,
        amount: paymentIntent.amount,
        currency: paymentIntent.currency,
      });
    } catch (error) {
      fastify.log.error(error);
      reply.status(500).send({ error: error.message });
    }
  });
  

// Lancer le serveur
const start = async () => {
  try {
    await fastify.listen({ port: 3000 });
    console.log('API en cours d\'exécution sur http://localhost:3000');
  } catch (err) {
    fastify.log.error(err);
    process.exit(1);
  }
};
start();
