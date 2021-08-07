public static class CrossPath{
	private static void notify(){
		Buzzer.play(sTurnNotGreen);
		Led.on(cTurnNotGreen);
	}

	public static void findLineLeft(FloorRoute.FollowLine Follower){
		CrossPath.findLineBase(Follower, new ActionHandler[]{() => Servo.left(), () => Servo.right(), () => Servo.nextAngleLeft(10)}, -90);
	}

	public static void findLineRight(FloorRoute.FollowLine Follower){
		CrossPath.findLineBase(Follower, new ActionHandler[]{() => Servo.right(), () => Servo.left(), () => Servo.nextAngleRight(10)}, 90);
	}

	private static void findLineBase(FloorRoute.FollowLine Follower, ActionHandler[] turnsCallback, float maxDegrees){
		if((Follower.lastCrossPath.millis + 256) > Time.current.millis){Buzzer.play(sMultiplesCross);return;}
		CrossPath.notify();
		Log.clear();
		Log.proc();
		Degrees max = new Degrees(Gyroscope.x.raw + maxDegrees);
		Servo.encoder(7f);
		Servo.rotate(-(maxDegrees / 9));
		turnsCallback[0]();
		while(true){
			if(CrossPath.checkLine(Follower)){Follower.lastCrossPath = Time.current;Time.resetTimer();return;}
			if(Gyroscope.x % max){
				max = new Degrees(Gyroscope.x.raw - (maxDegrees * 2));
				turnsCallback[1]();
				while (true){
					if(CrossPath.checkLine(Follower)){Follower.lastCrossPath = Time.current;Time.resetTimer();return;}
					if (Gyroscope.x % max){
						Servo.stop();
						Servo.encoder(-6f);
						max = new Degrees(Gyroscope.x.raw + (maxDegrees / 5));
						turnsCallback[0]();
						while(true){
							if(Gyroscope.x % max){
								Servo.encoder(6f);
								turnsCallback[2]();
								Servo.encoder(2.5f);
								Servo.rotate(maxDegrees);
								Follower.lastCrossPath = Time.current;
								Time.resetTimer();
								return;
							}
							if(Follower.s1.hasLine() || Follower.s2.hasLine()){
								Servo.encoder(6f);
								turnsCallback[2]();
								Servo.encoder(2.5f);
								Servo.rotate(-maxDegrees);
								Follower.lastCrossPath = Time.current;
								Time.resetTimer();
								return;
							}
						}
					}
				}
			}
		}
	}

	public static bool checkLine(FloorRoute.FollowLine Follower){
		if(Follower.s1.light.raw < 55 && !Follower.s1.isColored() && !Follower.s1.isMat()){
			Servo.stop();
			Buzzer.play(sFindLine);
			Servo.rotate(-2f);
			return true;
		}
		if(Follower.s2.light.raw < 55 && !Follower.s2.isColored() && !Follower.s2.isMat()){
			Servo.stop();
			Buzzer.play(sFindLine);
			Servo.rotate(2f);
			return true;
		}
		return false;
	}

	public static bool verify(Reflective tsensor) => tsensor.light.raw < 45 && !tsensor.isMat() && !tsensor.isColored();
}
