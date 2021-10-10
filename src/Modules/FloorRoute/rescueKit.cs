static private class RescueKit {
	public static void verify(FloorRoute.FollowLine Follower) {
		if (s3.rgb.hasKit()) {
			RescueKit.capture();
		}
	}

	public static void backCapture(Degrees defaultDirection) {
		Log.proc();
		Servo.forward(120);
		Time.sleep(700);
		Servo.forward(80);
		Actuator.close();
		Actuator.alignUp();

		Servo.backward(300);
		Time.sleep(800);
		Servo.stop();
		Servo.alignToAngle(defaultDirection);
	}

	public static void capture() {
		Log.proc();
		Degrees saveDirection = Gyroscope.x;
		Servo.alignNextAngle();
		Servo.backward(300);
		Time.sleep(350);
		Servo.stop();

		Actuator.open();
		Actuator.alignDown();

		int timeout = Time.current.millis + 1000;
		Servo.forward(165);
		while (!Actuator.kit) {
			if (Time.current.millis >= timeout) {
				Log.info($"Actuator.kit: {Actuator.kit}");
				Log.debug("Timeout capture");
				RescueKit.backCapture(saveDirection);
				return;
			}
		}
		Log.info($"Actuator.kit: {Actuator.kit}");
		Log.debug("Rescue Kit founded");
		RescueKit.backCapture(saveDirection);
	}
}
