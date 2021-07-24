public class FollowLine{
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, ref Reflective refs3_, ref Reflective refs4_, ref Reflective refs5_, int velocity_){
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
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw} | {this.s3.light.raw} | {this.s4.light.raw} | {this.s5.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
		Led.on(cFollowLine);
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		Green.verify(this);
		CrossPath.verify(this);
		if(this.s2.light.raw < 52 && !this.s2.isColored()){
			Servo.left();
			while(this.s3.light.raw > 50){Green.verify(this);CrossPath.verify(this);}
			Time.sleep(48, () => {Green.verify(this);CrossPath.verify(this);});
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
			Time.resetTimer();
		}else if(this.s4.light.raw < 52 && !this.s4.isColored()){
			Servo.right();
			while(this.s3.light.raw > 50){Green.verify(this);CrossPath.verify(this);}
			Time.sleep(48, () => {Green.verify(this);CrossPath.verify(this);});
			Servo.stop();
			Servo.foward(this.velocity);
			Time.sleep(32, () => {Green.verify(this);CrossPath.verify(this);});
			Time.resetTimer();
		}else{
			Servo.foward(this.velocity);
			Security.verify(this);
		}
	}

	public void alignSensors(){
		if(this.s2.light > this.s4.light){
			Servo.left();
			while(this.s3.light.raw < 50){}
			Time.sleep(32);
			Servo.stop();
		}else{
			Servo.right();
			while(this.s3.light.raw < 50){}
			Time.sleep(32);
			Servo.stop();
		}
	}
}
