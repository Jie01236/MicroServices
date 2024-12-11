from flask import Flask
from database import init_app, db
from routes import register_routes

def create_app():
    app = Flask(__name__)

    init_app(app)

    register_routes(app)

    return app

if __name__ == "__main__":
    app = create_app()
    with app.app_context():
        db.create_all()  # Crée la base de données et les tables
    app.run(host="0.0.0.0", port=7000, debug=True)
