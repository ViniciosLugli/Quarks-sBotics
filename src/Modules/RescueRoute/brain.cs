public class RescueBrain{
	public RescueBrain(){
		victimsAnnihilator = new VictimsAnnihilator(this);
	}

	public VictimsAnnihilator victimsAnnihilator;
	public Degrees defaultEnterDegrees;

	private const short RESCUE_SIZE = 300;
	private RescueInfo rescue = new RescueInfo();

	private void findExit(sbyte exitIndex, int maxTime = 600, ActionHandler callback = null){
		Log.proc();
		Time.resetTimer();
		while(Time.timer.millis < maxTime){
			Servo.antiLifting();
			Servo.foward(200);
			if(uRight.distance.raw > RESCUE_SIZE){
				rescue.setExit(exitIndex);
				callback?.Invoke();

			}
			Log.info(Formatter.parse($"uRight: {uRight.distance.raw}, speed: {Servo.speed()}, time: {maxTime - Time.timer.millis}", new string[]{"i","color=#505050"}));
			Log.debug($"FINDING EXIT {exitIndex}");
		}
		Servo.stop();
		Log.clear();
	}

	public void findTriangleArea(){
		findExit(1);
		Log.debug("FINDING TRIANGLE 3");
		Servo.ultraGoTo(40, ref uFrontal, () => {
			if(this.rescue.setTriangle(3) && this.rescue.exit == 0){
				this.rescue.setExit(2);
			}
		});

		Log.clear();
		if(this.rescue.exit == 1){
			this.rescue.setTriangle(2);
		}

		if(!this.rescue.hasInfos()){
			Servo.alignNextAngle();
			Servo.rotate(-180);
			Servo.alignNextAngle();
			findExit(3, 500);
			Servo.backward(200);
			Time.sleep(500);
			Servo.stop();
			if(this.rescue.setExit(2)){
				this.rescue.setTriangle(1);
			}
			if(this.rescue.triangle == 0){
				Servo.rotate(-90);
				Servo.alignNextAngle();
				Log.debug("FINDING TRIANGLE 2");
				Servo.ultraGoTo(40, ref uFrontal, () => {
					this.rescue.setTriangle(2);
				});
				Log.clear();
			}
			if(this.rescue.triangle == 0){
				this.rescue.setTriangle(1);
			}
		}else{
			Log.info($"Triangle: {this.rescue.triangle}, Exit: {this.rescue.exit}");
			Log.debug("HAS INFOS!");
		}
		Servo.alignNextAngle();
	}

	public void goToCenter(){
		Servo.ultraGoTo((300 / 2) - (Robot.kDiffFrontalDistance * Robot.kErrorDelta), ref uFrontal, null, 300);
		Servo.nextAngleLeft(50);
		Servo.ultraGoTo((300 / 2) - (Robot.kDiffFrontalDistance * Robot.kErrorDelta), ref uFrontal, null, 300);
	}

	public class VictimsAnnihilator{
		public Distance distThreshold;
		public float counterThreshold;

		private RescueRoute.RescueBrain tBrain;
		private byte verifyCounter = 0;
		private List<float> verifyOccurrences = new List<float>();
		private Degrees lastPoint;

		public VictimsAnnihilator(RescueRoute.RescueBrain tBrainInstance, float defaultDist = 130f, float defaultCounter = 3.2f){
			this.distThreshold = new Distance(defaultDist);
			this.counterThreshold = defaultCounter;
			this.tBrain = tBrainInstance;
		}

		private bool checkDiagonal(){
			float localDegrees = Calc.toBearing(Gyroscope.x.raw + 90 - this.tBrain.defaultEnterDegrees.raw);
			if(this.tBrain.rescue.triangle == 1){
				return localDegrees > (135 - 24) && localDegrees < (135 + 24);
			}else if(this.tBrain.rescue.triangle == 2){
				return localDegrees > (45 - 24) && localDegrees < (45 + 24);
			}else if(this.tBrain.rescue.triangle == 3){
				return localDegrees > (315 - 24) && localDegrees < (315 + 24);
			}
			return false;
		}

		private bool checkVerifyP(Distance cDistance, byte serial){
			if(this.verifyCounter >= serial){
				if(this.checkDiagonal()){
					if(this.verifyOccurrences.Count == 0){
						return true;
					}
					this.verifyOccurrences.Sort();
					float[] occurrences = this.verifyOccurrences.ToArray();
					try{
						float first = this.verifyOccurrences[1];
						float last = this.verifyOccurrences[occurrences.Length - 1];

						if(Math.Abs(first - last) <= 8){
							Log.debug($"Math.Abs(first - last): {Math.Abs(first - last)}");
							return true;
						}
					}finally{
						Led.on(255, 255, 0);
					}
				}else{
					return true;
				}
			}
			return false;
		}

		private void resetInstances(){
			this.verifyCounter = 0;
			this.verifyOccurrences.Clear();
		}

		public Distance[] find(){
			Distance localDistance = uFrontal.distance;
			byte serial = 255;
			this.resetInstances();
			while(true){
				Servo.right();

				localDistance = uRight.distance;
				serial = (byte)(((this.distThreshold - localDistance) + (Robot.kDiffFrontalDistance / 2)) / this.counterThreshold);

				Log.info(Formatter.parse($"FINDING VICTIM", new string[]{"i","color=#78DCE8"}));
				Log.debug(Formatter.parse($"uRight: {localDistance.raw}, verifyCounter: {this.verifyCounter}, serial: {serial}", new string[]{"i","color=#505050"}));

				if(localDistance < this.distThreshold){
					this.verifyCounter++;
					this.verifyOccurrences.Add(localDistance.raw);
					if(this.checkVerifyP(localDistance, serial)){
						this.lastPoint = new Degrees(Gyroscope.x.raw + 5);
						Led.on(0, 255, 0);
						this.resetInstances();
						break;
					}
				}else{
					this.resetInstances();
					Led.on(255, 0, 0);
				}

				if(Gyroscope.inPoint(true, 2) && Time.timer.millis > 328){
					Servo.ultraGoTo((300 / 2) - ((Robot.kDiffFrontalDistance * Robot.kErrorDelta) / 3), ref uFrontal, null, 300);
					Time.resetTimer();
				}

				Time.sleep(16);
			}
			Servo.stop();
			Log.clear();
			Log.info(Formatter.parse($"FINDED VICTIM", new string[]{"i","color=#A9DC76"}));
			Buzzer.play(sTurnGreen);
			return new Distance[]{new Distance(this.distThreshold - localDistance), localDistance};
		}

		private void alignRescue(){
			Log.clear();
			Log.proc();
			Servo.alignToAngle(new Degrees(this.tBrain.defaultEnterDegrees.raw + this.tBrain.rescue.triangleBaseDegrees()));
		}

		private bool capture(Distance realDist){
			Log.proc();
			float diffBack = 0f;
			if(realDist.raw <= 50){
				diffBack = (50 - realDist.raw) / 3f;
				Servo.encoder(-diffBack, 200);
			}
			Actuator.alignDown();
			Actuator.open();
			int cRotations = (int)(realDist.toRotations() * 0.95f);
			int lastRotations = 0;
			Time.resetTimer();
			for (int rotation = 0; rotation < cRotations; rotation++){
				Log.info(Formatter.parse($"cRotations: {cRotations}, currentFor: {rotation}", new string[]{"i","color=#A9DC76"}));
				Servo.foward(150);
				Time.sleep(48);
				lastRotations = rotation;
				if(Actuator.victim){
					break;
				}else if(Servo.speed() < 1f && Time.timer.millis >= 192){
					lastRotations = lastRotations - 4;
				}
			}
			Time.sleep(128);
			Clock saveTimeCost = Time.current;
			Actuator.close();
			Actuator.alignUp();
			int timeToReturn = (int)(Time.current - saveTimeCost);
			Servo.stop();
			Log.proc();
			Log.debug(Formatter.parse($"Actuator.victim: {Actuator.victim}", new string[]{"i","color=#FFEA79"}));
			if(!Actuator.victim){
				Servo.nextAngleRight();
				this.tBrain.goToCenter();
				Servo.alignToAngle(this.lastPoint);
				return false;
			}
			Servo.backward(300);
			Time.sleep((int)(((lastRotations * 48) + timeToReturn + 192) * 0.60f));
			Servo.stop();
			return true;
		}

		public void rescue(Distance diffDist, Distance realDist){
			Log.proc();
			Servo.rotate((float)((88 + (diffDist.raw / 35))));
			if(!this.capture(realDist)){return;}
			this.alignRescue();
			Log.proc();
			Servo.foward(200);
			Time.sleep(192);
			Time.resetTimer();
			while(Servo.speed() > 0.5f || Time.timer.millis <= 312){
				Log.debug(Formatter.parse($"speed: {Servo.speed()}, timer: {Time.timer.millis}", new string[]{"i","color=#FFEA79"}));
				Servo.antiLiftingRescue();
			}
			Time.sleep(64);
			Servo.stop();
			Actuator.open();
			Actuator.dropVictim();
			for (int i = 0; i < 2; i++){
				Servo.backward();
				Time.sleep(128);
				Servo.foward();
				Time.sleep(128);
			}
				Servo.stop();
			Actuator.close();
			Actuator.alignUp();
			Servo.nextAngleRight();
			this.tBrain.goToCenter();
			Servo.alignToAngle(this.lastPoint);
		}
	}
}
