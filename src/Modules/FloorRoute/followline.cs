public class FollowLine{
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2;
	private int velocity = 0;

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		Green.verify(this);
		if((this.s1.light.raw < 50) && !this.s1.isMat()){
			Servo.left();
			Time.resetTimer();
			while(this.s1.rgb.r < 60){
				Green.verify(this);
				if(Time.timer.millis > 112){
					Green.verify(this);
					if(Gyroscope.inPoint() && CrossPath.verify(this.s1)){
						CrossPath.findLineLeft(ref this.s1);
					}
					break;
				}
			}
			Time.sleep(48, () => Green.verify(this));
			Servo.foward(this.velocity);
			Time.sleep(32, () => Green.verify(this));
			Servo.stop();
			Time.sleep(Robot.kRefreshRate - 16, () => Green.verify(this));
			Green.verify(this);
			if(CrossPath.verify(this.s1)){
				CrossPath.findLineLeft(ref this.s1);
			}
			Green.verify(this);
		}else if((this.s2.light.raw < 50) && !this.s2.isMat()){
			Servo.right();
			Time.resetTimer();
			while(this.s2.rgb.r < 60){
				Green.verify(this);
				if(Time.timer.millis > 112){
					Green.verify(this);
					if(Gyroscope.inPoint() && CrossPath.verify(this.s2)){
						CrossPath.findLineRight(ref this.s2);
					}
					break;
				}
			}
			Time.sleep(48, () => Green.verify(this));
			Servo.foward(this.velocity);
			Time.sleep(32, () => Green.verify(this));
			Servo.stop();
			Time.sleep(Robot.kRefreshRate - 16, () => Green.verify(this));
			Green.verify(this);
			if(CrossPath.verify(this.s2)){
				CrossPath.findLineRight(ref this.s2);
			}
			Green.verify(this);
		}else{
			Servo.foward(this.velocity);
		}
	}

	public void alignSensors(bool right = true){
		if(right){
			Servo.right();
			while(!this.s1.hasLine()){}
			Servo.rotate(-4.5f);
		}else{
			Servo.left();
			while(!this.s2.hasLine()){}
			Servo.rotate(4.5f);
		}
	}
}
