import pika
import json

# Connection parameters
connection_params = pika.ConnectionParameters(
        host='localhost',  # RabbitMQ server address
        port=9000          # RabbitMQ server port
)



# Connexion à RabbitMQ
connection = pika.BlockingConnection(pika.ConnectionParameters('localhost'))
channel = connection.channel()

# Déclaration de l'échange pour les événements de réservation
channel.exchange_declare(exchange='reservations_exchange', exchange_type='fanout')

def publish_event(event_type, payload):
    message = {
        "type": event_type,
        "data": payload
    }
    channel.basic_publish(
        exchange='reservations_exchange',
        routing_key='',  # Fanout ne nécessite pas de clé
        body=json.dumps(message)
    )
    print(f"Événement publié : {message}")
