public static class CrossPath {
	private static void notify() {
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(FloorRoute.FollowLine Follower) {
		Log.clear();
		Log.info(Formatter.parse($"Align to left", new string[] { "i", "color=#505050" }));
		CrossPath.findLineBase(Follower, new ActionHandler[] { () => Servo.left(), () => Servo.right(), () => Servo.nextAngleLeft(10) }, -90);
	}

	public static void findLineRight(FloorRoute.FollowLine Follower) {
		Log.clear();
		Log.info(Formatter.parse($"Align to right", new string[] { "i", "color=#505050" }));
		CrossPath.findLineBase(Follower, new ActionHandler[] { () => Servo.right(), () => Servo.left(), () => Servo.nextAngleRight(10) }, 90);
	}

	private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler[] turnsCallback, float maxDegrees) {
		//if ((Follower.lastCrossPath.millis + 256) > Time.current.millis) { Buzzer.play(sMultiplesCross); return; }
		CrossPath.notify();
		Log.proc();
		Degrees max = new Degrees(Gyroscope.x.raw + maxDegrees);
		Servo.encoder(7f);
		Servo.rotate(-(maxDegrees / 27)); // Check line before turn, inveted axis!
		turnsCallback[0]();
		while (true) {
			if (CrossPath.checkLine(Follower)) { Follower.lastCrossPath = Time.current; Time.resetTimer(); return; }
			if (Gyroscope.x % max) {
				max = new Degrees(Gyroscope.x.raw - (maxDegrees * 2));
				turnsCallback[1]();
				while (true) {
					if (CrossPath.checkLine(Follower)) { Follower.lastCrossPath = Time.current; Time.resetTimer(); return; }
					if (Gyroscope.x % max) {
						Servo.stop();
						Servo.encoder(-6f);
						max = new Degrees(Gyroscope.x.raw + (maxDegrees / 5));
						turnsCallback[0]();
						while (true) {
							if (Gyroscope.x % max) {
								Servo.encoder(6f);
								turnsCallback[2]();
								Servo.encoder(2.5f);
								Servo.rotate(maxDegrees);
								Follower.lastCrossPath = Time.current;
								Time.resetTimer();
								return;
							}
							if (Follower.s1.hasLine() || Follower.s2.hasLine()) {
								Servo.encoder(6f);
								turnsCallback[2]();
								Servo.encoder(2.5f);
								Servo.rotate(-maxDegrees);
								Follower.lastCrossPath = Time.current;
								Time.resetTimer();
								return;
							}
						}
					}
				}
			}
		}
	}

	public static bool checkLine(FloorRoute.FollowLine Follower) {
		if (Follower.s1.light.raw < 52 && !Follower.s1.isMat()) {
			Servo.stop();
			Follower.checkEndLine();
			Buzzer.play(sFindLine);
			Servo.rotate(-4.5f);
			return true;
		}
		if (Follower.s2.light.raw < 52 && !Follower.s2.isMat()) {
			Servo.stop();
			Follower.checkEndLine();
			Buzzer.play(sFindLine);
			Servo.rotate(4.5f);
			return true;
		}
		return false;
	}

	public static bool verify(Reflective tsensor) => tsensor.light.raw < 50 && !tsensor.isMat();
}
