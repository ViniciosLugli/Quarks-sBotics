static private class Security{
	public static void verify(FloorRoute.FollowLine Follower){
		if(Time.timer.millis > (2800 - (Follower.velocity * 10))){
			if(Gyroscope.inPoint()){
				Security.checkInLine(Follower, () => Security.backToLine(Follower));
			}else{
				Security.backToLine(Follower);
			}
			Time.resetTimer();
		}
	}

	private static void backToLine(FloorRoute.FollowLine Follower){
		Servo.backward(Follower.velocity);
		while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){}
		Servo.stop();
		Servo.encoder(-3);
	}

	private static void checkInLine(FloorRoute.FollowLine Follower, MethodHandler callback){
		Clock rTimer = new Clock(Time.current.millis + 128);
		while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){
			Servo.rotate(-2f);
			callback();
		}
		Servo.rotate(-1.5f);
	}
}
