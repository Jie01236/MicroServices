import jwt
from datetime import datetime, timedelta
from flask import current_app
from werkzeug.security import generate_password_hash, check_password_hash
from models import User
from database import db

def generate_token(user):
    expiration = timedelta(hours=1)
    exp = datetime.utcnow() + expiration
    token = jwt.encode({
        'sub': user.id,
        'exp': exp
    }, current_app.config['SECRET_KEY'], algorithm='HS256')
    return token

def hash_password(password):
    return generate_password_hash(password)

def verify_password(hashed_password, password):
    return check_password_hash(hashed_password, password)

def authenticate_user(email, password):
    user = User.query.filter_by(email=email).first()
    if user and verify_password(user.hashed_password, password):
        return user
    return None
