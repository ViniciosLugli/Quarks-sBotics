public class Obstacle {
	public Obstacle(ref Ultrassonic refuObs_, byte distance_ = 15) {
		this.uObs = refuObs_;
		this.distance = distance_;
	}

	private Ultrassonic uObs;
	private byte distance;

	public void verify() {
		if (uObs.distance.raw > 16 && uObs.distance.raw < this.distance && Time.current.millis > 1500) {
			this.dodge();
			this.verify();
		}
	}

	public void dodge() {
		// Find line left
		Servo.stop();
		Servo.alignNextAngle();
		Servo.encoder(-4);
		Servo.rotate(-35);
		Servo.encoder(10);

		Servo.forward(200);
		int timeout = Time.current.millis + 256 + 64;
		while (Time.current.millis < timeout) {
			if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
				Servo.encoder(9);
				Servo.rotate(-10);
				Servo.encoder(7);
				Servo.nextAngleLeft(10);
				Servo.backward(300);
				Time.sleep(128);
				Servo.stop();
				return;
			}
		}

		// Find line right
		Servo.encoder(-20);
		Servo.rotate(35);
		Servo.alignNextAngle();

		Servo.rotate(35);
		Servo.encoder(10);

		Servo.forward(200);
		timeout = Time.current.millis + 256 + 64;
		while (Time.current.millis < timeout) {
			if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
				Servo.encoder(9);
				Servo.rotate(10);
				Servo.encoder(7);
				Servo.nextAngleRight(10);
				Servo.backward(300);
				Time.sleep(128);
				Servo.stop();
				return;
			}
		}
		Servo.stop();

		// Find line straight
		Servo.rotate(-20);
		Servo.encoder(14);
		Servo.rotate(-70);

		Servo.forward(200);
		timeout = Time.current.millis + 256 + 128;
		while (Time.current.millis < timeout) {
			if ((s1.hasLine() && !s1.isMat()) || (s2.hasLine() && !s2.isMat())) {
				Servo.encoder(9);
				Servo.rotate(10);
				Servo.encoder(7);
				Servo.nextAngleRight(10);
				Servo.backward(300);
				Time.sleep(128);
				Servo.stop();
				return;
			}
		}
		Servo.stop();
	}
}
