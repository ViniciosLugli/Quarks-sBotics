static private class Security{
	public static void verify(FloorRoute.FollowLine Follower){
		if(Time.timer.millis > (2800 - (Follower.velocity * 13)) && mainRescue.rampTimer == 0){
			Security.checkInLine(Follower, () => Security.backToLine(Follower));
			Time.resetTimer();
		}
	}

	private static void backToLine(FloorRoute.FollowLine Follower){
		Servo.backward(Follower.velocity);
		while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55)){}
		Servo.stop();
		Servo.encoder(-3);
	}

	private static void checkInLine(FloorRoute.FollowLine Follower, ActionHandler callback){
		Clock timeout = new Clock(Time.current.millis + 256);
		while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) ){
			Servo.left();
			if(Time.current > timeout){
				Servo.right();
				Time.sleep(256);
				Servo.stop();
				callback();
				return;
			}
		}
		Servo.right();
		Time.sleep(timeout - Time.current);
		Servo.stop();
	}
}
