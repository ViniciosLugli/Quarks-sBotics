public static class CrossPath{
	private static void notify(){
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 80);
		Servo.encoder(8f);
		Servo.left();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(2f);
	}

	public static void findLineRight(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 80);
		Servo.encoder(8f);
		Servo.right();
		while((!refsensor_.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(-2f);
	}

	public static bool verify(Reflective tsensor) => tsensor.light.raw < 45 || tsensor.rgb.r < 25;
}
