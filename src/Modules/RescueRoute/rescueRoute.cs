public class RescueRoute{
	import("Modules/RescueRoute/victim.cs");
	import("Modules/RescueRoute/rescueAnalyzer.cs");
	import("Modules/RescueRoute/RampFollowLine.cs");

	public RescueRoute(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		mainRampFollowLine = new RampFollowLine(ref refs1_, ref refs2_, velocity_);
	}

	private int rampTimer = 0;
	private RampFollowLine mainRampFollowLine;

	public void verify(){
		if(upRamp.isOnRange(Gyroscope.z) && (uRight.distance.raw < 40)){
			if(rampTimer == 0){
				rampTimer = Time.current.millis + 2000;

			}else if(Time.current.millis > rampTimer){
				Servo.stop();
				this.main();
			}
		}else{
			rampTimer = 0;
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
