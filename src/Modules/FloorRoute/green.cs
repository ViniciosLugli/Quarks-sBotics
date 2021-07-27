public class Green{

	private static void notify(){
		Buzzer.play(sTurnGreen);
		Led.on(cTurnGreen);
	}

	public static void findLineBack(){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(16f);
		Servo.rotate(180f);
	}


	public static void findLineLeft(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(-25f);
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 87);
		Servo.left();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(3f);
	}

	public static void findLineRight(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(25f);
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 87);
		Servo.right();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(-3f);
	}

	public static bool verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
			Follower.alignSensors();
			Servo.foward();
			Time.sleep(32);
			Servo.stop();
			if(Follower.s1.rgb.hasGreen() && Follower.s2.rgb.hasGreen()){
				Green.findLineBack();
			}else if(Follower.s1.rgb.hasGreen()){
				Green.findLineLeft(ref Follower.s2);
			}else if(Follower.s2.rgb.hasGreen()){
				Green.findLineRight(ref Follower.s1);
			}
			Follower.lastGreen = Time.current;
			Time.resetTimer();
			return true;
		}else{
			return false;
		}
	}
}
