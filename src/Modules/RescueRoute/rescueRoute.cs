public class RescueRoute{
	import("Modules/RescueRoute/victim.cs");
	import("Modules/RescueRoute/rescueAnalyzer.cs");
	import("Modules/RescueRoute/RampFollowLine.cs");

	public RescueRoute(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		mainRampFollowLine = new RampFollowLine(ref refs1_, ref refs2_, velocity_);
	}

	public int rampTimer = 0;
	private RampFollowLine mainRampFollowLine;

	public void verify(){
		if(upRamp.isOnRange(Gyroscope.z) && (uRight.distance.raw < 40)){
			if(this.rampTimer == 0){
				this.rampTimer = Time.current.millis + 2000;

			}else if(Time.current.millis > this.rampTimer){
				Servo.stop();
				this.main();
			}
		}else{
			this.rampTimer = 0;
		}
	}

	public void check(){
		if(upRamp.isOnRange(Gyroscope.z)){
			main();
		}
	}

	private void main(){
		RescueAnalyzer.setup();
		for(;;){
			while(upRamp.isOnRange(Gyroscope.z)){
				mainRampFollowLine.proc();
			}
			Servo.encoder(10);
			Time.debug();
		}
	}
}
