public class Green{

	private static void notify(){
		Buzzer.play(sTurnGreen);
		Led.on(cTurnGreen);
	}

	public static void findLineLeft(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(-30f);
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 87);
		Servo.left();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(-3f);
	}

	public static void findLineRight(ref Reflective refsensor_){
		Green.notify();
		Log.clear();
		Log.proc();
		Servo.encoder(14f);
		Servo.rotate(30f);
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 87);
		Servo.right();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(3f);
	}

	public static void verify(ref Reflective refs1_, ref Reflective refs2_){
		if(refs1_.rgb.hasGreen() || refs2_.rgb.hasGreen()){
			Position.alignSensors();
			Time.sleep(32);
			if(refs1_.rgb.hasGreen()){
				findLineLeft(ref refs1_);
			}else if(refs2_.rgb.hasGreen()){
				findLineRight(ref refs2_);
			}
		}
	}
}
