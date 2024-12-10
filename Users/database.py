from flask_sqlalchemy import SQLAlchemy

db = SQLAlchemy()

def init_app(app):
    app.config.from_object('config.Config')
    db.init_app(app)
