public class Obstacle{
	public Obstacle(ref Ultrassonic refuObs_, byte distance_ = 15){
		this.uObs = refuObs_;
		this.distance = distance_;
	}

	private Ultrassonic uObs;
	private byte distance;

	public void verify(){
		if(uObs.distance.raw > 16 && uObs.distance.raw < this.distance && Time.current.millis > 3000){
			this.dodge();
			this.verify();
		}
	}

	public void dodge(){
		Servo.stop();
		Servo.alignNextAngle();
		Servo.rotate(58);
		Servo.encoder(7);
		Servo.rotate(-27);
		Servo.encoder(7);
		Servo.rotate(-15);
		Servo.encoder(9);
		Servo.rotate(-10);
		Servo.encoder(17);
		Servo.nextAngleLeft(10);
		Servo.encoder(12);
		Servo.nextAngleRight(10);
	}
}
