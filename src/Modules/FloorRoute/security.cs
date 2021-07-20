static private class Security{
	public static void verify(FloorRoute.FollowLine Follower){
		if((Time.timer.millis > 2000) && !Gyroscope.inPoint()){
			Servo.backward(180);
			while(!(Follower.s1.light.raw < 55) && !(Follower.s2.light.raw < 55) && !(Follower.s3.light.raw < 55) && !(Follower.s4.light.raw < 55)){}
			Servo.stop();
			Servo.encoder(-3);
		}
	}
}
