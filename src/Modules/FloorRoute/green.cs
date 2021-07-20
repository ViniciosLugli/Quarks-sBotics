public class Green{

	private static void notify(){
		Buzzer.play(sTurnGreen);
		Led.on(cTurnGreen);
	}

	public static void findLineLeft(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(-20f);
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 87);
		Servo.left();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(-3f);
	}

	public static void findLineRight(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(20f);
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 87);
		Servo.right();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(3f);
	}

	public static void verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
			Follower.alignSensors();
			Time.sleep(32);
			if(Follower.s1.rgb.hasGreen()){
				findLineLeft(ref Follower.s1);
			}else if(Follower.s2.rgb.hasGreen()){
				findLineRight(ref Follower.s2);
			}
		}
	}
}
