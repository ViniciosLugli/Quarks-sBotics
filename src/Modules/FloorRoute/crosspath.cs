private static class CrossPath{
	private static void notify(){
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw - 80);
		Degrees max = new Degrees(Gyroscope.x.raw - 100);
		Servo.encoder(7f);
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
	}

	public static void findLineRight(ref Reflective refsensor_){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw + 80);
		Degrees max = new Degrees(Gyroscope.x.raw + 100);
		Servo.encoder(7f);
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
	}

	public static void verify(FloorRoute.FollowLine Follower){
		if(Follower.s1.light.raw < 52 && !Follower.s1.isMat()){
			if (Green.verify(Follower)) { Time.resetTimer(); return; }
			findLineLeft(ref Follower.s3);
			Time.resetTimer();
		}else if(Follower.s5.light.raw < 52 && !Follower.s5.isMat()){
			if (Green.verify(Follower)) { Time.resetTimer(); return; }
			findLineRight(ref Follower.s3);
			Time.resetTimer();
		}
	}
}
