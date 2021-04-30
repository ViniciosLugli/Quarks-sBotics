class Actuator{
	//Public:
	public static void position(float obj_degrees, int velocity){
		bc.ActuatorSpeed(velocity);

		int timeout = Time.millis() + (3000 - (velocity*10));
		float local_angle = bc.AngleActuator();

		obj_degrees = (obj_degrees < 0 || obj_degrees > 300) ? 0 : (obj_degrees > 88) ? 88 : obj_degrees;

		if(obj_degrees > local_angle){
			while(obj_degrees > local_angle){
				bc.ActuatorUp(1);
				if(Time.millis() > timeout){return;}
				local_angle = bc.AngleActuator();
			}
		}else if(obj_degrees < local_angle){
			while(obj_degrees < local_angle){
				bc.ActuatorDown(1);
				if(Time.millis() > timeout){return;}
				local_angle = bc.AngleActuator();
			}
		}
	}

	public static void degrees(float obj_degrees, int velocity){
		bc.ActuatorSpeed(velocity);

		int timeout = Time.millis() + (3000 - (velocity*10));
		float local_angle = bc.AngleScoop();

		obj_degrees = (obj_degrees < 0 || obj_degrees > 300) ? 0 : (obj_degrees > 12) ? 12 : obj_degrees;

		if(obj_degrees > local_angle){
			while(obj_degrees > local_angle){
				bc.TurnActuatorDown(1);
				if(Time.millis() > timeout){return;}
				local_angle = bc.AngleScoop();
			}
		}else if(obj_degrees < local_angle){
			while(obj_degrees < local_angle){
				bc.TurnActuatorUp(1);
				if(Time.millis() > timeout){return;}
				local_angle = bc.AngleScoop();
			}
		}
	}
};
