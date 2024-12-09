const sqlite3 = require('sqlite3').verbose();
const db = new sqlite3.Database('./payments.db');

db.all('SELECT * FROM payments', (err, rows) => {
  if (err) {
    console.error('Erreur lors de la récupération des données', err);
  } else {
    console.log('Contenu de la base de données :');
    console.table(rows);
  }
  db.close();
});
