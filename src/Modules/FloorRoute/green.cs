private class Green{

	private static void notifyGreen(){
		Buzzer.play(sTurnGreen);
		Led.on(cTurnGreen);
	}

	private static void notifyFakeGreen(){
		Buzzer.play(sFakeGreen);
		Led.on(cFakeGreen);
	}

	private static void findLineBack(FloorRoute.FollowLine Follower){
		Log.clear();
		Log.proc();
		Servo.encoder(13f);
		Servo.rotate(180);
	}

	private static void findLineLeft(FloorRoute.FollowLine Follower){
		Log.clear();
		Log.proc();
		Servo.encoder(8f);
		Servo.rotate(-15f);
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 88);
		Servo.left();
		while((!Follower.s3.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(-2f);
	}

	private static void findLineRight(FloorRoute.FollowLine Follower){
		Log.clear();
		Log.proc();
		Servo.encoder(8f);
		Servo.rotate(15f);
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 88);
		Servo.right();
		while((!Follower.s3.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(2f);
	}

	public static void confirm(FloorRoute.FollowLine Follower, MethodHandler callback){
		Clock bTimer = new Clock(Time.current.millis + 256);
		Servo.foward(Follower.velocity);
		while(bTimer > Time.current){
			if((Follower.s1.light.raw < 52 && !Follower.s1.isColored()) || (Follower.s2.light.raw < 52 && !Follower.s2.isColored()) || (Follower.s4.light.raw < 52 && !Follower.s4.isColored()) || (Follower.s5.light.raw < 52 && !Follower.s5.isColored())){
				Servo.stop();
				Green.notifyGreen();
				callback();
				return;
			}
		}
		Servo.stop();
		Green.notifyFakeGreen();
	}

	public static bool verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen() || Follower.s3.rgb.hasGreen()  || Follower.s4.rgb.hasGreen() || Follower.s5.rgb.hasGreen()){
			Follower.alignSensors();
			Time.sleep(32);

			if((Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()) && (Follower.s4.rgb.hasGreen() || Follower.s5.rgb.hasGreen())){
				Green.confirm(Follower, () => Green.findLineBack(Follower));

			}else if(Follower.s1.rgb.hasGreen() || Follower.s2.rgb.hasGreen()){
				Green.confirm(Follower, () => Green.findLineLeft(Follower));

			}else if(Follower.s4.rgb.hasGreen() || Follower.s5.rgb.hasGreen()){
				Green.confirm(Follower, () => Green.findLineRight(Follower));
			}
			Time.resetTimer();
			return true;
		}
		return false;
	}
}
