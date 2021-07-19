public class FollowLine{
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.velocity = velocity_;
	}

	private Reflective s1, s2;
	private int velocity = 0;

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
		Log.debug($"{Formatter.parse("--", new string[]{$"mark={this.s1.light.toHex()}"})} | {Formatter.parse("--", new string[]{$"mark={this.s2.light.toHex()}"})}");
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		Green.verify(ref this.s1, ref this.s2);
		if(this.s1.rgb.r < 52 || this.s1.light.raw < 55){
			Servo.left();
			Time.resetTimer();
			while(this.s1.rgb.r < 52 || this.s1.light.raw < 55){
				Green.verify(ref this.s1, ref this.s2);
				if(Time.timer.millis > 128){
					break;
				}
			}
			Time.sleep(16);
			Servo.foward(this.velocity);
			Time.sleep(16);
			Servo.stop();
			Time.sleep(kRefreshRate - 16);
			if (CrossPath.verify(this.s1)){
				CrossPath.findLineLeft(ref this.s2);
			}
			Green.verify(ref this.s1, ref this.s2);
		}else if(this.s2.rgb.r < 52 || this.s2.light.raw < 55){
			Servo.right();
			Time.resetTimer();
			while(this.s2.rgb.r < 52 || this.s2.light.raw < 55){
				Green.verify(ref this.s1, ref this.s2);
				if (Time.timer.millis > 128){
					break;
				}
			}
			Time.sleep(16);
			Servo.foward(this.velocity);
			Time.sleep(16);
			Servo.stop();
			Time.sleep(kRefreshRate - 16);
			if (CrossPath.verify(this.s2)){
				CrossPath.findLineRight(ref this.s1);
			}
			Green.verify(ref this.s1, ref this.s2);
		}else{
			Servo.foward(this.velocity);
		}
	}
}
