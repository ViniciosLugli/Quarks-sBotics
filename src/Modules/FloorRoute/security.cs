static private class Security {
	public static void verify(FloorRoute.FollowLine Follower) {
		if (Time.timer.millis > (2800 - (Follower.velocity * 13)) && mainRescue.rampTimer == 0 && !floor.isOnRange(Gyroscope.z)) {
			Security.checkInLine(Follower, () => Security.backToLine(Follower));
			Time.resetTimer();
		}
	}

	private static void backToLine(FloorRoute.FollowLine Follower) {
		while (!Security.findLine(Follower)) { Servo.encoder(-6); }
	}

	private static void checkInLine(FloorRoute.FollowLine Follower, ActionHandler callback) {
		Clock timeout = new Clock(Time.current.millis + 256);
		while (!(Follower.s1.light.raw < 55 && !Follower.s1.isMat()) && !(Follower.s2.light.raw < 55 && !Follower.s2.isMat())) {
			Servo.left();
			if (Time.current > timeout) {
				Servo.right();
				Time.sleep(256);
				Servo.stop();
				callback();
				return;
			}
		}
		Servo.right();
		Time.sleep(timeout - Time.current);
		Servo.stop();
	}

	private static bool findLine(FloorRoute.FollowLine Follower) {
		Degrees defaultAxis = Gyroscope.x;
		Degrees max = new Degrees(defaultAxis.raw - 10);

		Func<Degrees, bool> findLineBase = (degrees) => {
			while (!(Gyroscope.x % degrees)) {
				if (CrossPath.checkLine(Follower)) {
					Servo.stop();
					return true;
				}
			}
			return false;
		};

		Servo.left();
		if (findLineBase(max)) { return true; }
		Servo.stop();

		max = new Degrees(defaultAxis.raw + 20);
		Servo.right();
		if (findLineBase(max)) { return true; }
		Servo.stop();

		Servo.left();
		if (findLineBase(defaultAxis)) { return true; }
		Servo.stop();
		return false;
	}
}
