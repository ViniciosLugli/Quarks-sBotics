public class Green{

	private static void notify(int index = 0){
		Buzzer.play(sTurnGreen, index);
		Led.on(cTurnGreen);
	}

	public static void findLineBack(){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(16f);
		Servo.rotate(180f);
	}

	public static void findLineLeft(FloorRoute.FollowLine Follower){
		Green.findLineBase(Follower, () => Servo.left(), -25, -87);
	}

	public static void findLineRight(FloorRoute.FollowLine Follower){
		Green.findLineBase(Follower, () => Servo.right(), 25, 87);
	}

	private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler turnCallback, float ignoreDegrees, float maxDegrees){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(ignoreDegrees);
		turnCallback();
		while(true){
			if(CrossPath.checkLine(Follower)){ break; }
			if(Gyroscope.inPoint(true, 3)){ Servo.encoder(-5); break; }
		}
		Servo.stop();
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
				Green.findLineLeft(Follower);
			}else if(Follower.s2.rgb.hasGreen()){
				Green.findLineRight(Follower);
			}
			Follower.lastGreen = Time.current;
			Time.resetTimer();
			return true;
		}else{
			return false;
		}
	}
}
