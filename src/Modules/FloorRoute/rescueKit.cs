static private class RescueKit {
	public static void verify(FloorRoute.FollowLine Follower) {
		if (s3.rgb.hasKit()) {
			Degrees saveDirection = Gyroscope.x;
			Servo.alignNextAngle();
			Servo.backward(300);
			Time.sleep(400);
			Servo.stop();

			Actuator.open();
			Actuator.alignDown();

			int timeout = Time.current.millis + 2000;
			while (!Actuator.kit) {
				if (Time.current.millis >= timeout) {
					Actuator.close();
					Actuator.alignUp();

					Servo.backward(300);
					Time.sleep(400);
					Servo.stop();
					break;
				}
				Servo.forward(165);
			}
			Servo.forward(130);
			Time.sleep(900);
			Servo.stop();
			Actuator.close();
			Actuator.alignUp();

			Servo.backward(300);
			Time.sleep(500);
			Servo.stop();
			Servo.alignToAngle(saveDirection);
		}
	}
}
