public class RampFollowLine{
	public RampFollowLine(ref Reflective refs1_, ref Reflective refs2_, ref Reflective refs3_, ref Reflective refs4_, ref Reflective refs5_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.s3 = refs3_;
		this.s4 = refs4_;
		this.s5 = refs5_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2, s3, s4, s5;
	public int velocity = 0;

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw} | {this.s3.light.raw} | {this.s4.light.raw}| {this.s5.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));
		Led.on(cRampFollowLine);
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		if(((50 - this.s2.light.raw) > 35) || (s1.light.raw < 45)){
			Servo.left();
			Time.sleep(32);
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(16);
		}
		else if(((50 - this.s4.light.raw) > 35) || (s5.light.raw < 45)){
			Servo.right();
			Time.sleep(32);
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(16);
		}
		else{
			Servo.foward(this.velocity);
		}
	}
}
