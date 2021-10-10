public static class Actuator {
	public static void position(float degrees, int velocity = 150) {
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity * 10));
		float local_angle = bc.AngleActuator();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 88) ? 88 : degrees;

		Log.proc();

		if (degrees > local_angle) {
			while (degrees > local_angle) {
				bc.ActuatorUp(32);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		} else if (degrees < local_angle) {
			while (degrees < local_angle) {
				bc.ActuatorDown(32);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static void angle(float degrees, int velocity = 150) {
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (2000 - (velocity * 10));
		float local_angle = bc.AngleScoop();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 12) ? 12 : degrees;

		Log.proc();

		if (degrees > local_angle) {
			while (degrees > local_angle) {
				bc.TurnActuatorDown(32);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		} else if (degrees < local_angle) {
			while (degrees < local_angle) {
				bc.TurnActuatorUp(32);
				if (Time.current.millis > timeout) { return; }
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static bool victim {
		get => bc.HasVictim();
	}

	public static bool kit {
		get => bc.HasRescueKit();
	}

	public static void open() {
		Log.clear();
		Log.proc();
		bc.OpenActuator();
	}

	public static void close() {
		Log.clear();
		Log.proc();
		bc.CloseActuator();
	}

	public static void alignUp() {
		position(88);
		angle(0);
	}
	public static void alignDown() {
		position(0);
		angle(0);
	}

	public static void dropVictim() {
		position(0);
		angle(10);
	}

	public static void NOP() {
		Log.clear();
		Log.proc();
		bc.AngleActuator();
		bc.AngleScoop();
	}
}
