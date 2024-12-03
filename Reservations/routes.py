from flask import request, jsonify
from models import Reservation, db  

def register_routes(app):
    @app.route("/reservations", methods=["POST"])
    def create_reservation():
        data = request.json
        if not data:
            return jsonify({"error": "Request must contain JSON data"}), 400

        # verify the content of jsonfile
        required_fields = ["user_id", "property_id", "check_in", "check_out", "guests", "total_price"]
        for field in required_fields:
            if field not in data:
                return jsonify({"error": f"Missing required field: {field}"}), 400

        # create new reservation
        new_reservation = Reservation(
            user_id=data["user_id"],
            property_id=data["property_id"],
            check_in=data["check_in"],
            check_out=data["check_out"],
            guests=data["guests"],
            total_price=data["total_price"],
        )
        db.session.add(new_reservation)
        db.session.commit()
        return jsonify({"message": "Reservation created", "reservation_id": new_reservation.id}), 201

    @app.route("/reservations", methods=["GET"])
    def get_reservations():
        user_id = request.args.get("user_id")
        reservations = Reservation.query.filter_by(user_id=user_id).all()
        result = [
            {
                "id": r.id,
                "property_id": r.property_id,
                "check_in": r.check_in,
                "check_out": r.check_out,
                "guests": r.guests,
                "total_price": r.total_price,
                "status": r.status,
            }
            for r in reservations
        ]
        return jsonify(result), 200

    @app.route("/reservations/<int:reservation_id>", methods=["DELETE"])
    def cancel_reservation(reservation_id):
        reservation = Reservation.query.get_or_404(reservation_id)
        reservation.status = "cancelled"
        db.session.commit()
        return jsonify({"message": "Reservation cancelled", "reservation_id": reservation.id}), 200
