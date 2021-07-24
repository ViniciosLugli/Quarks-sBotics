static private class Security{
	public static void verify(FloorRoute.FollowLine Follower){
		if((Time.timer.millis > (2800 - (Follower.velocity * 10))) && Follower.s3.light.raw > 55){
			Security.backToLine(Follower);
			Time.resetTimer();
		}
	}

	private static void backToLine(FloorRoute.FollowLine Follower){
		Servo.backward(Follower.velocity);
		while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){}
		Servo.stop();
		Servo.encoder(-3);
	}
}
