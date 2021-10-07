static private class Security {
	public static void verify(FloorRoute.FollowLine Follower) {
		if (Time.timer.millis > 96) {
			if (!Servo.SmoothAlignNextAngle(Follower)) {
				Follower.resetMovement();
			}
			return;
		}
		Follower.resetMovement();
	}
}
