
# Documentation de l’API du Service de Réservation

## 1. Vue d’ensemble
Le service de réservation est un microservice qui gère les réservations des utilisateurs. 
Il offre les fonctionnalités suivantes :

- **Créer une nouvelle réservation**  
- **Consulter l’historique des réservations d’un utilisateur**  
- **Annuler une réservation**  

---

## 2. Liste des APIs

| Fonctionnalité              | Méthode | URL                                     | Description                            |
|-----------------------------|---------|-----------------------------------------|----------------------------------------|
| Créer une réservation       | POST    | `/reservations`                        | Créer une nouvelle réservation.        |
| Consulter l’historique      | GET     | `/reservations?user_id=<user_id>`       | Consulter l’historique des réservations d’un utilisateur. |
| Annuler une réservation      | DELETE  | `/reservations/<reservation_id>`        | Annuler une réservation spécifique.    |

---

## 3. Détails des APIs

### 1. Créer une réservation
- **URL** : `/reservations`  
- **Méthode** : POST  

**Exemple de requête** :  
```json
{
    "user_id": 2,
    "property_id": 102,
    "check_in": "2024-12-15",
    "check_out": "2024-12-19",
    "guests": 2,
    "total_price": 300
}
```

**Exemple de réponse** :  
- **Succès (201 Created)** :  
```json
{
    "message": "Reservation created",
    "reservation_id": 3
}
```

- **Erreur (400 Bad Request)** :  
```json
{
    "error": "Missing required field: user_id"
}
```

---

### 2. Consulter l’historique des réservations
- **URL** : `/reservations?user_id=<user_id>`  
- **Méthode** : GET  

**Exemple de réponse** :  
- **Succès (200 OK)** :  
```json

{
    "id": 3,
    "property_id": 102,
    "check_in": "2024-12-15",
    "check_out": "2024-12-19",
    "guests": 2,
    "total_price": 300,
    "status": "confirmed"
}

```

- **Erreur (400 Bad Request)** :  
```json
{
    "error": "Missing user_id parameter"
}
```

---

### 3. Annuler une réservation
- **URL** : `/reservations/<reservation_id>`  
- **Méthode** : DELETE  

**Exemple de réponse** :  
- **Succès (200 OK)** :  
```json
{
    "message": "Reservation cancelled",
    "reservation_id": 3
}
```

- **Erreur (404 Not Found)** :  
```json
{
    "error": "Reservation not found"
}
```

---

## 4. Codes d’erreur

| Code d’erreur | Description                                |
|---------------|--------------------------------------------|
| 400           | Requête invalide ou paramètres manquants. |
| 404           | Ressource non trouvée (ex. réservation inexistante). |
| 500           | Erreur interne du serveur.                |

---

## 5. Remarques
- Les valeurs dans les exemples de requêtes et réponses sont fictives et servent uniquement à illustrer l’utilisation de l’API.
- Assurez-vous que l’API est testée via Postman ou un outil similaire pour vérifier son bon fonctionnement.
