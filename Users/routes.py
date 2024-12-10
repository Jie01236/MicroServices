from flask import request, jsonify
from auth import hash_password, authenticate_user, generate_token
from models import User, db
from messaging import publish_message 


def register_routes(app):

    @app.route("/register", methods=["POST"])
    def register_user():
        data = request.json
        if not data:
            return jsonify({"error": "Request must contain JSON data"}), 400

        required_fields = ["username", "email", "password"]
        for field in required_fields:
            if field not in data:
                return jsonify({"error": f"Missing required field: {field}"}), 400

        # Check if the user already exists
        if User.query.filter_by(email=data["email"]).first():
            return jsonify({"error": "Email already registered"}), 400

        hashed_password = hash_password(data["password"])
        new_user = User(username=data["username"], email=data["email"], hashed_password=hashed_password)
        db.session.add(new_user)
        db.session.commit()

        # Publier un message dans RabbitMQ
        publish_message("user_notifications", f"New user registered: {data['email']}")

        return jsonify({"message": "User registered successfully", "user_id": new_user.id}), 201

    @app.route("/login", methods=["POST"])
    def login_user():
        data = request.json
        if not data:
            return jsonify({"error": "Request must contain JSON data"}), 400

        user = authenticate_user(data["email"], data["password"])
        if not user:
            return jsonify({"error": "Invalid credentials"}), 401

        token = generate_token(user)
        return jsonify({"access_token": token, "token_type": "bearer"}), 200

    @app.route("/user/<int:user_id>", methods=["GET"])
    def get_user(user_id):
        user = User.query.get_or_404(user_id)
        return jsonify({
            "id": user.id,
            "username": user.username,
            "email": user.email,
            "created_at": user.created_at
        }), 200
