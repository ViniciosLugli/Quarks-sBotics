class Actuator{
	public static void position(float degrees, int velocity){
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity*10));
		float local_angle = bc.AngleActuator();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 88) ? 88 : degrees;

		Log.proc($"Actuator | position({degrees}, {velocity})");

		if(degrees > local_angle){
			while(degrees > local_angle){
				bc.ActuatorUp(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		}else if(degrees < local_angle){
			while(degrees < local_angle){
				bc.ActuatorDown(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleActuator();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static void angle(float degrees, int velocity){
		Log.clear();
		bc.ActuatorSpeed(velocity);

		int timeout = Time.current.millis + (3000 - (velocity*10));
		float local_angle = bc.AngleScoop();

		degrees = (degrees < 0 || degrees > 300) ? 0 : (degrees > 12) ? 12 : degrees;

		Log.proc($"Actuator | angle({degrees}, {velocity})");

		if(degrees > local_angle){
			while(degrees > local_angle){
				bc.TurnActuatorDown(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		}else if(degrees < local_angle){
			while(degrees < local_angle){
				bc.TurnActuatorUp(1);
				if(Time.current.millis > timeout){return;}
				local_angle = bc.AngleScoop();
				Log.info($"local_angle: {local_angle}");
			}
		}
	}

	public static void open() {Log.clear();Log.proc($"Actuator | open()");bc.OpenActuator();}

	public static void close() {Log.clear();Log.proc($"Actuator | close()");bc.CloseActuator();}

	public static bool victim {
		get{
			return bc.HasVictim();
		}
	}

	public static void NOP(){
		Log.clear();
		Log.proc($"Actuator | NOP()");
		bc.AngleActuator();
		bc.AngleScoop();
	}
};
