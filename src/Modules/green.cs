class Green{

	public static bool isGreen(Color color){
		float rgb = color.r + color.g + color.b;
		byte pR = (byte)Calc.map(color.r, 0, rgb, 0, 100);
		byte pG = (byte)Calc.map(color.g, 0, rgb, 0, 100);
		byte pB = (byte)Calc.map(color.b, 0, rgb, 0, 100);
		return ((pG > pR) && (pG > pB) && (pG > 70));
	}

	public static void findLineLeft(){
		Log.clear();
		Log.proc($"Green | findLineLeft()");
		Servo.encoder(10f);
		Servo.rotate(-30f);
		Servo.left();
		while(!s3.hasLine()){}
		Servo.stop();
		Servo.rotate(0.5f);
	}

	public static void findLineRight(){
		Log.clear();
		Log.proc($"Green | findLineRight()");
		Servo.encoder(10f);
		Servo.rotate(30f);
		Servo.right();
		while(!s2.hasLine()){}
		Servo.stop();
		Servo.rotate(0.5f);
	}

	public static void verify(){
		if(isGreen(s1.rgb) || isGreen(s2.rgb) || isGreen(s3.rgb) || isGreen(s4.rgb)){
			Position.alignSensors();
			Time.sleep(32);
			if(isGreen(s1.rgb) || isGreen(s2.rgb)){
				findLineLeft();
			}else if(isGreen(s3.rgb) || isGreen(s4.rgb)){
				findLineRight();
			}
		}
	}
}
