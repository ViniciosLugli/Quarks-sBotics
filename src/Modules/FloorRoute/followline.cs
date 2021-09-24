public class FollowLine {
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_) {
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2;
	public int velocity = 0;
	public Clock lastGreen = new Clock(0);
	public Clock lastCrossPath = new Clock(0);

	private void debugSensors() {
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));
	}

	public void proc() {
		Log.proc();
		this.debugSensors();

		if (Green.verify(this)) { return; }

		if (checkSensor(ref this.s1, () => Servo.left(), () => CrossPath.findLineLeft(this))) { } else if (checkSensor(ref this.s2, () => Servo.right(), () => CrossPath.findLineRight(this))) { } else { Servo.foward(this.velocity); Security.verify(this); }
	}

	private bool checkSensor(ref Reflective refsensor_, ActionHandler correctCallback, ActionHandler crossCallback) {
		if (refsensor_.light.raw < 55 && !refsensor_.isColored() && !refsensor_.isMat()) {
			correctCallback();
			Clock timeout = new Clock(Time.current.millis + 128);
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
			Servo.foward(this.velocity);
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
