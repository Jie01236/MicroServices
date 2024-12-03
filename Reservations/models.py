from flask_sqlalchemy import SQLAlchemy
from database import db

class Reservation(db.Model):
    id = db.Column(db.Integer, primary_key=True)
    user_id = db.Column(db.Integer, nullable=False)
    property_id = db.Column(db.Integer, nullable=False)
    check_in = db.Column(db.String(50), nullable=False)
    check_out = db.Column(db.String(50), nullable=False)
    guests = db.Column(db.Integer, nullable=False)
    total_price = db.Column(db.Float, nullable=False)
    status = db.Column(db.String(50), default="confirmed")  # "confirmed" or "cancelled"
