public class RescueRoute {
	import("Modules/RescueRoute/victim.cs");
	import("Modules/RescueRoute/rescueAnalyzer.cs");
	import("Modules/RescueRoute/brain.cs");

	private RescueBrain brain = new RescueBrain();

	private void InitRescue() {
		Servo.stop();
		Buzzer.play(sFindLine);
		Time.sleep(50);
		Buzzer.play(sFindLine);
		this.main();
	}

	public void verify() {
		if (s1.isRescueEnter() && s2.isRescueEnter()) {
			Servo.stop();
			this.InitRescue();
		} else if (s1.isRescueEnter()) {
			Servo.stop();
			Servo.left();
			while (!s2.isRescueEnter()) { }
			Servo.stop();
			this.InitRescue();

		} else if (s2.isRescueEnter()) {
			Servo.stop();
			Servo.right();
			while (!s1.isRescueEnter()) { }
			Servo.stop();
			this.InitRescue();
		}
	}

	private void main() {
		//RescueAnalyzer.setup();

		Servo.encoder(5);
		Servo.alignNextAngle();
		this.brain.defaultEnterDegrees = Gyroscope.x;
		Servo.encoder(25);

		this.brain.findTriangleArea();
		Led.on(0, 0, 255);
		for (; ; ) {
			this.brain.victimsAnnihilator.find();
		}
	}
}
