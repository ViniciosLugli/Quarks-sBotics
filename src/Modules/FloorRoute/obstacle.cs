public class Obstacle{
	public Obstacle(ref Ultrassonic refuObs_, byte distance_ = 15){
		this.uObs = refuObs_;
		this.distance = distance_;
	}

	private Ultrassonic uObs;
	private byte distance;

	public void verify(){
		if(uObs.distance.raw < this.distance){
			this.dodge();
			this.verify();
		}
	}

	public void dodge(){//TODO: Double obstacle...
		Servo.stop();
		Servo.alignNextAngle();
		Servo.rotate(58);
		Servo.encoder(7);
		Servo.rotate(-25);
		Servo.encoder(7);
		Servo.rotate(-15);
		Servo.encoder(9);
		Servo.rotate(-10);
		Servo.encoder(12);
		Servo.rotate(-20);
		Servo.encoder(5);
		Servo.rotate(-30);
		Servo.encoder(3);
		Servo.rotate(-30);
		Servo.encoder(3);
		Servo.rotate(10);
		Servo.encoder(6);
		Servo.nextAngleRight();
	}
}
