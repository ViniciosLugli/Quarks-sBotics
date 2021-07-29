static private class Security{
	public static void verify(FloorRoute.FollowLine Follower){
		if(Time.timer.millis > (2800 - (Follower.velocity * 13)) && mainRescue.rampTimer == 0){
			Security.checkInLine(Follower, () => Security.backToLine(Follower));
			Time.resetTimer();
		}
	}

	private static void backToLine(FloorRoute.FollowLine Follower){
		while(!Security.findLine(Follower)){Servo.encoder(-5);}
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

	private static bool findLine(FloorRoute.FollowLine Follower){
		Degrees max = new Degrees(Gyroscope.x.raw - 20);
		Servo.left();
		while (!(Gyroscope.x % max)){
			if(CrossPath.checkLine(Follower)){
				Servo.stop();
				return true;
			}
		}
		Servo.stop();
		max = new Degrees(Gyroscope.x.raw + 20);
		Servo.right();
		while(!(Gyroscope.x % max)){
			if(CrossPath.checkLine(Follower)){
				Servo.stop();
				return true;
			}
		}
		Servo.stop();
		return false;
	}
}
