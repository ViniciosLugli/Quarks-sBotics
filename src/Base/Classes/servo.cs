public static class Servo {
	public static void move(float left = 300, float right = 300) => bc.Move(left, right);

	public static void foward(float velocity = 300) => bc.Move(Math.Abs(velocity), Math.Abs(velocity));

	public static void backward(float velocity = 300) => bc.Move(-velocity, -velocity);

	public static void left(float velocity = 1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity = 1000) => bc.Move(+velocity, -velocity);

	public static void rotate(float angle, float velocity = 500) => bc.MoveFrontalAngles(velocity, angle);
	public static void rotate(Degrees angle, float velocity = 500) => bc.MoveFrontalAngles(velocity, angle.raw);

	public static void encoder(float rotations, float velocity = 300) => bc.MoveFrontalRotations(rotations > 0 ? velocity : -velocity, Math.Abs(rotations));

	public static float speed() => bc.RobotSpeed();

	public static void stop() => bc.Move(0, 0);

	public static void antiLifting() {
		if ((Gyroscope.isLifted() || !Gyroscope.inPoint(true, 6)) && Time.timer.millis > 312) {
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 378;
			while (Gyroscope.isLifted() && Time.current.millis < timeout) {
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Servo.alignNextAngle();
			Time.resetTimer();
		}
	}

	public static void antiLiftingRescue() {
		if ((Gyroscope.isLifted() || !Gyroscope.inDiagonal()) && Time.timer.millis >= 312) {
			Log.proc();
			Buzzer.play(sLifting);
			Servo.stop();
			int timeout = Time.current.millis + 312;
			while (Gyroscope.isLifted() && Time.current.millis < timeout) {
				Servo.backward(200);
			}
			Time.sleep(128);
			Servo.stop();
			Time.resetTimer();
		}
	}

	public static void nextAngleRight(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(Math.Abs(ignoreAngles));
		Servo.right();
		while (!Gyroscope.inPoint(false)) { }
		Servo.stop();
	}

	public static void nextAngleLeft(byte ignoreAngles = 0) {
		Log.proc();
		Servo.rotate(-ignoreAngles);
		Servo.left();
		while (!Gyroscope.inPoint(false)) { }
		Servo.stop();
	}

	public static void alignNextAngle() {
		Log.proc();
		if (Gyroscope.inPoint(true, 2)) { return; }
		Degrees alignLocal = new Degrees(0);
		if ((Gyroscope.x.raw > 315) || (Gyroscope.x.raw <= 45)) {
			alignLocal = new Degrees(0);
		} else if ((Gyroscope.x.raw > 45) && (Gyroscope.x.raw <= 135)) {
			alignLocal = new Degrees(90);
		} else if ((Gyroscope.x.raw > 135) && (Gyroscope.x.raw <= 225)) {
			alignLocal = new Degrees(180);
		} else if ((Gyroscope.x.raw > 225) && (Gyroscope.x.raw <= 315)) {
			alignLocal = new Degrees(270);
		}

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[] { "i", "color=#505050" }));

		if ((alignLocal.raw == 0) && (Gyroscope.x.raw > 180)) {
			Servo.right();
		} else if ((alignLocal.raw == 0) && (Gyroscope.x.raw < 180)) {
			Servo.left();
		} else if (Gyroscope.x < alignLocal) {
			Servo.right();
		} else if (Gyroscope.x > alignLocal) {
			Servo.left();
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

	public static void alignToAngle(object angle) {
		Log.proc();

		Degrees alignLocal = (angle is Degrees) ? (Degrees)angle : new Degrees((float)angle);

		Log.info(Formatter.parse($"Align to {alignLocal.raw}°", new string[] { "i", "color=#505050" }));

		float baseFind = Calc.toBearing(Gyroscope.x - alignLocal);

		if (baseFind >= 180) {
			Servo.right();
		} else if (baseFind < 180) {
			Servo.left();
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

	private static bool ultraGoToRecursive(Ultrassonic ultra, ActionHandler callback) {
		Log.info(Formatter.parse($"ultra: {ultra.distance.raw}, speed: {Servo.speed()}", new string[] { "i", "color=#505050" }));
		Servo.antiLifting();
		if (Servo.speed() < 0.5f && Time.timer.millis > 500) {
			callback?.Invoke();
			return true;
		}
		return false;
	}

	public static void ultraGoTo(float position, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200) {
		Log.proc();
		Time.resetTimer();
		if (position > ultra.distance.raw) {
			while (position > ultra.distance.raw) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.backward(velocity);
			}
		} else {
			while (position < ultra.distance.raw) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.foward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}

	public static void ultraGoTo(Distance dist, ref Ultrassonic ultra, ActionHandler callback = null, int velocity = 200) {
		Log.proc();
		Time.resetTimer();
		if (dist > ultra.distance) {
			while (dist > ultra.distance) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.backward(velocity);
			}
		} else {
			while (dist < ultra.distance) {
				if (ultraGoToRecursive(ultra, callback)) { break; }
				Servo.foward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}
}
