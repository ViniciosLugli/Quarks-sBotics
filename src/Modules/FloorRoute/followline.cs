public class FollowLine{
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, ref Reflective refs3_, ref Reflective refs4_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.s3 = refs3_;
		this.s4 = refs4_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2, s3, s4;
	private int velocity = 0;

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw} | {this.s3.light.raw} | {this.s4.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		Green.verify(this);
		CrossPath.verify(this);
		if((50 - this.s2.light.raw) > 20){
			Servo.left();
			Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(16, () => {Green.verify(this);CrossPath.verify(this);});
			Time.resetTimer();
		}else if((50 - this.s3.light.raw) > 20){
			Servo.right();
			Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(16, () => {Green.verify(this);CrossPath.verify(this);});
			Time.resetTimer();
		}else{
			Servo.foward(this.velocity);
			Security.verify(this);
		}
	}

	public void alignSensors(bool right = true){
		if(right){
			Servo.right();
			while(!this.s2.hasLine()){}
			Servo.rotate(-2f);
		}else{
			Servo.left();
			while(!this.s3.hasLine()){}
			Servo.rotate(2f);
		}
	}
}
