import("Utils/derivative");

class FollowLine{

	private float kP = 0, P = 0;
	private int error = 0;

	public FollowLine(float kP_){
		this.kP = kP_;
	}

	private float sensorsError() => (float)Math.Round((s2.light.value - s3.light.value), 2);

	public void proc(float velocity){
		Log.proc($"FollowLine | proc({velocity})");

		// Log.info($"{s1.light.value} | {s2.light.value} | {s3.light.value} | {s4.light.value}");

		error = (int)this.sensorsError();

		P = error * this.kP;
		float leftVel = (float)Math.Round(velocity - P, 2);
		float rightVel = (float)Math.Round(velocity + P, 2);
		Log.info($"rightVel: {rightVel}, leftVel: {leftVel}");

		if(rightVel >= 200 && leftVel <= -200){
			Log.debug($"LEFT");
			Servo.left();
			Time.sleep(44);
		}else if(rightVel <= -200 && leftVel >= 200){
			Log.debug($"RIGHT");
			Servo.right();
			Time.sleep(44);
		}else{
			Log.debug($"PROP");
			Servo.move(leftVel, rightVel);
		}
	}
}
