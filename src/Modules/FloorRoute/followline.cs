public class FollowLine{
	public FollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2;
	public int velocity = 0;
	public Clock lastGreen = new Clock(0);

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[]{"align=center", "color=#FFEA79", "b"}));
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
		if(Green.verify(this)){return;}
		if(this.s1.light.raw < 55 && !this.s1.isColored()){
			Servo.left();
			Time.resetTimer();
			while(this.s1.light.raw < 55){
				if(Green.verify(this)){return;}
				if(Time.timer.millis > 144){
					if(Green.verify(this)){return;}
					if(Gyroscope.inPoint() && CrossPath.verify(this.s1)){
						CrossPath.findLineLeft(this);
						return;
					}
					break;
				}
			}
			Time.sleep(32, () => Green.verify(this));
			Servo.foward(this.velocity);
			Time.sleep(32, () => Green.verify(this));
			Servo.stop();
			if(Green.verify(this)){return;}
			if((this.lastGreen.millis + 320) > Time.current.millis){
				return;
			}else if(CrossPath.verify(this.s1)){
				CrossPath.findLineLeft(this);
				return;
			}
			if(Green.verify(this)){return;}

		}else if(this.s2.light.raw < 55 && !this.s2.isColored()){
			Servo.right();
			Time.resetTimer();
			while(this.s2.light.raw < 55){
				if(Green.verify(this)){return;}
				if(Time.timer.millis > 144){
					if(Green.verify(this)){return;}
					if(Gyroscope.inPoint() && CrossPath.verify(this.s2)){
						CrossPath.findLineRight(this);
						return;
					}
					break;
				}
			}
			Time.sleep(32, () => Green.verify(this));
			Servo.foward(this.velocity);
			Time.sleep(32, () => Green.verify(this));
			Servo.stop();
			if(Green.verify(this)){return;}
			if((this.lastGreen.millis + 320) > Time.current.millis){
				return;
			}else if(CrossPath.verify(this.s2)){
				CrossPath.findLineRight(this);
				return;
			}
			if(Green.verify(this)){return;}
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
