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
		this.checkEndLine();

		if (mainObstacle.verify(this)) { return; }

		if (Green.verify(this)) { return; }

		if (checkSensor(ref this.s1, () => Servo.left(), () => CrossPath.findLineLeft(this))) {
			return;
		} else if (checkSensor(ref this.s2, () => Servo.right(), () => CrossPath.findLineRight(this))) {
			return;
		} else {
			Servo.move(this.moveVelocity);
			Security.verify(this);
			RescueKit.verify(this);
		}
	}

	public void checkEndLine() {
		if (this.s1.isEndLine() || this.s2.isEndLine()) {
			Servo.stop();
			Servo.encoder(2);
			if (this.s1.isEndLine() || this.s2.isEndLine()) {
				Servo.stop();
				Servo.encoder(7);
				Log.clear();
				Led.on(255, 0, 0);
				Log.custom(0, Formatter.parse($"----------------------------------------", new string[] { "align=center", "color=#FF6188", "b" }));
				Log.custom(1, Formatter.parse($"!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!", new string[] { "align=center", "color=#FFEA79", "b" }));
				Log.custom(2, Formatter.parse($"----------------------------------------", new string[] { "align=center", "color=#FF6188", "b" }));
				Time.debug();
			}
			Servo.encoder(-2);
		}
	}

	private bool checkSensor(ref Reflective refsensor_, ActionHandler correctCallback, ActionHandler crossCallback) {
		if (refsensor_.light.raw < 52 && !refsensor_.isMat()) {
			Clock timeout = new Clock(Time.current.millis + 128 + 64);
			correctCallback();
			while (refsensor_.light.raw < 52) {
				mainRescue.verify();
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
			Servo.forward(this.defaultVelocity);
			Time.sleep(32, () => { Green.verify(this); mainRescue.verify(); });
			mainRescue.verify();
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
