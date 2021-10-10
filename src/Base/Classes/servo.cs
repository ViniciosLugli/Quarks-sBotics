import("Base/Structs/direction.cs");

public static class Servo {
	public static void move(Direction direction) => bc.Move(direction.left, direction.right);
	public static void move(float left = 300, float right = 300) => bc.Move(left, right);

	public static void forward(float velocity = 300) => bc.Move(Math.Abs(velocity), Math.Abs(velocity));

	public static void backward(float velocity = 300) => bc.Move(-velocity, -velocity);

	public static void left(float velocity = 1000) => bc.Move(-velocity, +velocity);

	public static void right(float velocity = 1000) => bc.Move(+velocity, -velocity);


	public static void rotate(float angle, float velocity = 1000) {
		Degrees alignLocal = new Degrees(Gyroscope.x.raw + angle);
		if (angle > 0) {
			Servo.right(velocity);
		} else {
			Servo.left(velocity);
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}
	public static void rotate(Degrees angle, float velocity = 1000) {
		Degrees alignLocal = new Degrees(Gyroscope.x.raw + angle.raw);
		if (angle.raw > 0) {
			Servo.right(velocity);
		} else {
			Servo.left(velocity);
		}
		while (!(Gyroscope.x % alignLocal)) { }
		Servo.stop();
	}

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

	public static bool SmoothAlignNextAngle(FloorRoute.FollowLine Follower) {

		if (Gyroscope.inPoint(true, 1) || Gyroscope.inDiagonal(true, 1)) { return false; }

		Degrees alignLocal = new Degrees(0);

		float? temp = Gyroscope.inRawPoint(true, 22);

		if (temp is null) {
			temp = Gyroscope.inRawDiagonal(true, 10);
			if (temp is null) {
				return false;
			}
		}
		alignLocal = new Degrees((float)temp);

		float diffDegrees = Math.Abs(alignLocal.raw - Gyroscope.x.raw);

		Log.debug(Formatter.parse($"Smooth to {alignLocal.raw}° | Diff: {diffDegrees}°", new string[] { "i", "color=#505050" }));

		diffDegrees = diffDegrees * cDegreesMovementProp;

		void leftMove() {
			Direction filtred = cLeftMovement;
			if (diffDegrees > 16) {
				filtred.right = (int)(filtred.right - 60);
				filtred.left = (int)(filtred.left);
			} else {
				filtred.right = (int)(filtred.right - diffDegrees);
			}
			Follower.moveVelocity = filtred;
		}

		void rightMove() {
			Direction filtred = cRightMovement;
			if (diffDegrees > 16) {
				filtred.left = (int)(filtred.left - 60);
				filtred.right = (int)(filtred.right);
			} else {
				filtred.left = (int)(filtred.left - diffDegrees);
			}
			Follower.moveVelocity = filtred;
		}

		if ((alignLocal.raw == 0) && (Gyroscope.x.raw > 180)) {
			leftMove();
		} else if ((alignLocal.raw == 0) && (Gyroscope.x.raw < 180)) {
			rightMove();
		} else if (Gyroscope.x < alignLocal) {
			leftMove();
		} else if (Gyroscope.x > alignLocal) {
			rightMove();
		} else {
			Log.proc();
			Log.info(Formatter.parse("Oh fuck 2", new string[] { "i", "color=#505050" }));
		}
		return true;
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
				Servo.forward(velocity);
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
				Servo.forward(velocity);
			}
		}
		Servo.stop();
		Log.clear();
	}
}
