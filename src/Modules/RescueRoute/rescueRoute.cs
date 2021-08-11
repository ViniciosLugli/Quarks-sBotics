public class RescueRoute {
	import("Modules/RescueRoute/rescueInfo.cs");
	import("Modules/RescueRoute/victim.cs");
	import("Modules/RescueRoute/rescueAnalyzer.cs");
	import("Modules/RescueRoute/rampFollowline.cs");
	import("Modules/RescueRoute/brain.cs")

	public RescueRoute(ref Reflective refs1_, ref Reflective refs2_, int velocity_) {
		mainRampFollowLine = new RampFollowLine(ref refs1_, ref refs2_, velocity_);
	}

	public int rampTimer = 0;
	private RampFollowLine mainRampFollowLine;

	private RescueBrain brain = new RescueBrain();

	public void verify() {
		if (upRamp.isOnRange(Gyroscope.z) && (uRight.distance.raw < 42)) {
			if (this.rampTimer == 0) {
				this.rampTimer = Time.current.millis + 2000;

			} else if (Time.current.millis > this.rampTimer) {
				Servo.stop();
				this.main();
			}
		} else {
			this.rampTimer = 0;
		}
	}

	public void check() {
		if (upRamp.isOnRange(Gyroscope.z)) {
			main();
		}
	}

	private void main() {
		RescueAnalyzer.setup();
		while (upRamp.isOnRange(Gyroscope.z) && uRight.distance.raw < 42) {
			mainRampFollowLine.proc();
		}
		Servo.alignNextAngle();
		for (byte i = 0; i < 4; i++) {
			Servo.encoder(3, 200);
			Servo.encoder(-1, 200);
		}
		Servo.alignNextAngle();
		this.brain.defaultEnterDegrees = Gyroscope.x;
		Servo.ultraGoTo(260, ref uFrontal);

		this.brain.findTriangleArea();
		Led.on(0, 0, 255);
		this.brain.goToCenter();
		Distance[] localVictimInfoRescue;
		for (; ; ) {
			localVictimInfoRescue = this.brain.victimsAnnihilator.find();
			this.brain.victimsAnnihilator.rescue(localVictimInfoRescue[0], localVictimInfoRescue[1]);
			//this.brain.victimsAnnihilator.checkExit();
		}
	}
}
