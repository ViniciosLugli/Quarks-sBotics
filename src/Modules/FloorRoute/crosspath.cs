public static class CrossPath{
	private static void notify(){
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(FloorRoute.FollowLine Follower){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw - 75);
		Degrees max = new Degrees(Gyroscope.x.raw - 90);
		Servo.encoder(7f);
		Servo.left();
		while(true){
			if(CrossPath.checkLine(Follower)){return;}
			if(Gyroscope.x % max){
				max = new Degrees(Gyroscope.x.raw + 165);
				Servo.right();
				while(true){
					if(CrossPath.checkLine(Follower)){return;}
					if (Gyroscope.x % max){
						Servo.left();
						while(!(Gyroscope.x % initialDefault)){}
						Servo.stop();
						return;
					}
				}
			}
		}
	}

	public static void findLineRight(FloorRoute.FollowLine Follower){
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees initialDefault = new Degrees(Gyroscope.x.raw + 75);
		Degrees max = new Degrees(Gyroscope.x.raw + 90);
		Servo.encoder(7f);
		Servo.right();
		while(true){
			if(CrossPath.checkLine(Follower)){return;}
			if(Gyroscope.x % max){
				max = new Degrees(Gyroscope.x.raw - 165);
				Servo.left();
				while (true){
					if(CrossPath.checkLine(Follower)){return;}
					if (Gyroscope.x % max){
						Servo.right();
						while(!(Gyroscope.x % initialDefault)){}
						Servo.stop();
						return;
					}
				}
			}
		}
	}

	public static bool checkLine(FloorRoute.FollowLine Follower){
		if(Follower.s1.light.raw < 55 && !Follower.s1.isColored()){
			Buzzer.play(sFindLine);
			Servo.stop();
			Servo.rotate(-2f);
			return true;
		}
		if(Follower.s2.light.raw < 55 && !Follower.s2.isColored()){
			Buzzer.play(sFindLine);
			Servo.stop();
			Servo.rotate(2f);
			return true;
		}
		return false;
	}

	public static bool verify(Reflective tsensor) => tsensor.light.raw < 45 && !tsensor.isColored();
}
