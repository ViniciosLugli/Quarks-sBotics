/*--------------------------------------------------
Code by Quarks - SESI CE 349
Last change: 30/04/2021 | 10:11:28
--------------------------------------------------*/

class Time{
	public static int millis(){
		return bc.Millis();
	}

	public static int timer(){
		return bc.Timer();
	}

	public static void resetTimer(){
		bc.ResetTimer();
	}

	public static void sleep(int ms){
		bc.Wait(ms);
	}

	public static bool await(object tester){
		return tester();
	}
};
class Calc{
	public static float constrain(float amt,float low,float high){
		return ((amt)<(low)?(low):((amt)>(high)?(high):(amt)));
	}

	public static float convert_degress(float degreesToConvert){
		return (degreesToConvert + 360) % 360;
	}

	public static float map(float value, float min, float max, float minTo, float maxTo){
		return (int)( (((value - min) * (maxTo - minTo)) / (max - min)) + minTo);
	}
};
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

bool tester(){
	return true;
}

void Main(){
	if(Time.await(tester)){
		actuator.position(88, 150);
	}
}
