public static class CrossPath{
	private static void notify(){
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw - 75);
		Degrees max = new Degrees(Gyroscope.x.raw - 80);
		Servo.encoder(8f);
		Servo.left();
		while((!refsensor_.hasLine())){
			if(Gyroscope.x % max){
				max = new Degrees(Gyroscope.x.raw + 165);
				Servo.encoder(-6f);
				Servo.right();
				while(true){
					if(refsensor_.hasLine()){
						Servo.rotate(-17);
						Servo.encoder(5f);
						return;
					}
					if (Gyroscope.x % max){
						Servo.left();
						while(!(Gyroscope.x % initialDefault)){}
						Servo.stop();
						return;
					}
				}
			}
		}
		Servo.stop();
		Servo.rotate(-5f);
	}

	public static void findLineRight(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw + 75);
		Degrees max = new Degrees(Gyroscope.x.raw + 80);
		Servo.encoder(8f);
		Servo.right();
		while(!refsensor_.hasLine()){
			if(Gyroscope.x % max){
				max = new Degrees(Gyroscope.x.raw - 165);
				Servo.encoder(-6f);
				Servo.left();
				while (true){
					if(refsensor_.hasLine()){
						Servo.rotate(17);
						Servo.encoder(5f);
						return;
					}
					if (Gyroscope.x % max){
						Servo.right();
						while(!(Gyroscope.x % initialDefault)){}
						Servo.stop();
						return;
					}
				}
			}
		}
		Servo.stop();
		Servo.rotate(5f);
	}

	public static bool verify(Reflective tsensor) => tsensor.light.raw < 50 && !tsensor.isMat();
}
