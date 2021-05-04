class CrossPath{
	public static void findLineLeft(){
		Log.clear();
		Log.proc($"CrossPath | findLineLeft()");
		Degrees maxLeft = new Degrees(Gyroscope.x.raw - 80);
		Servo.encoder(6f);
		Servo.left();
		while((!s3.hasLine()) && (!(Gyroscope.x % maxLeft))){}
		Servo.stop();
		Servo.rotate(0.5f);
	}

	public static void findLineRight(){
		Log.clear();
		Log.proc($"CrossPath | findLineRight()");
		Degrees maxRight = new Degrees(Gyroscope.x.raw + 80);
		Servo.encoder(6f);
		Servo.right();
		while((!s2.hasLine()) && (!(Gyroscope.x % maxRight))){}
		Servo.stop();
		Servo.rotate(0.5f);
	}

	public static void verify(){
		if((s1.light.value > 60) && (s2.light.value > 55)){
			findLineLeft();
		}else if((s4.light.value > 60) && (s3.light.value > 55)){
			findLineRight();
		}
	}
}
