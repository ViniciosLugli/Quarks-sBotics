public class FollowLine {
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int defaultVelocity_) {
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.defaultVelocity = defaultVelocity_;
		this.moveVelocity = new Direction(defaultVelocity_, defaultVelocity_);
	}

	public Reflective s1, s2;
	public int defaultVelocity = 0;
	public Direction moveVelocity = new Direction(0, 0);
	public Clock lastGreen = new Clock(0);
	public Clock lastCrossPath = new Clock(0);

	private void debugSensors() => Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));

	public void resetMovement() => this.moveVelocity = new Direction(this.defaultVelocity, this.defaultVelocity);

	public void proc() {
		Log.proc();
		this.debugSensors();

		if (Green.verify(this)) { return; }

		if (checkSensor(ref this.s1, () => Servo.left(), () => CrossPath.findLineLeft(this))) {

		} else if (checkSensor(ref this.s2, () => Servo.right(), () => CrossPath.findLineRight(this))) {

		} else {
			Servo.move(this.moveVelocity);
			Security.verify(this);
			RescueKit.verify(this);
		}
	}

	private bool checkSensor(ref Reflective refsensor_, ActionHandler correctCallback, ActionHandler crossCallback) {
		if (refsensor_.light.raw < 55 && !refsensor_.isMat()) {
			correctCallback();
			Clock timeout = new Clock(Time.current.millis + 128 + 16);
			while (refsensor_.light.raw < 55) {
				if (Green.verify(this)) { return true; }
				if (Time.current > timeout) {
					if (Green.verify(this)) { return true; }
					if (CrossPath.verify(refsensor_)) {
						crossCallback();
						return true;
					}
					break;
				}
			}
			Time.sleep(32, () => Green.verify(this));
			Servo.forward(this.defaultVelocity);
			Time.sleep(32, () => Green.verify(this));
			Servo.stop();
			if (Green.verify(this)) { return true; }
			if ((this.lastGreen.millis + 320) > Time.current.millis) {
				return true;
			} else if (CrossPath.verify(refsensor_) && !refsensor_.isColored()) {
				crossCallback();
			}
			if (Green.verify(this)) { return true; }
			Time.resetTimer();
			return true;
		}
		return false;
	}

	public void alignSensors(bool right = true) {
		if (right) {
			Servo.right();
			while (!(this.s1.light.raw < 55) || this.s1.isColored() || this.s1.isMat()) { }
			Servo.rotate(-4.5f);
		} else {
			Servo.left();
			while (!(this.s2.light.raw < 55) || this.s2.isColored() || this.s2.isMat()) { }
			Servo.rotate(4.5f);
		}
	}
}
