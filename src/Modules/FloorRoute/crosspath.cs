private static class CrossPath{
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
		Servo.rotate(-2f);
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
		Servo.rotate(2f);
	}

	public static void verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.light.raw < 50 && !Follower.s1.isMat()){
			findLineLeft(ref Follower.s2);
			Time.resetTimer();
		}else if(Follower.s4.light.raw < 50 && !Follower.s4.isMat()){
			findLineRight(ref Follower.s3);
			Time.resetTimer();
		}
	}
}
