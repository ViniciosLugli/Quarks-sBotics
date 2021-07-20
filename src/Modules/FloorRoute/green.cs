private class Green{

	private static void notify(){
		Buzzer.play(sTurnGreen);
		Led.on(cTurnGreen);
	}

	public static void findLineLeft(FloorRoute.FollowLine Follower){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(-20f);
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 87);
		Servo.left();
		while((!Follower.s2.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(-2f);
	}

	public static void findLineRight(FloorRoute.FollowLine Follower){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(20f);
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 87);
		Servo.right();
		while((!Follower.s3.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(2f);
	}

	public static void verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen() || Follower.s3.rgb.hasGreen() || Follower.s4.rgb.hasGreen()){
			Follower.alignSensors();
			Time.sleep(32);
			if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
				findLineLeft(Follower);
			}else if(Follower.s3.rgb.hasGreen() || Follower.s4.rgb.hasGreen()){
				findLineRight(Follower);
			}
			Time.resetTimer();
		}
	}
}
