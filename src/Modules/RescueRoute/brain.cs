
public class RescueBrain {

	public VictimsAnnihilator victimsAnnihilator;
	public Degrees defaultEnterDegrees;
	public bool currentSide;

	public ActionHandler[] actionMove = new ActionHandler[6];

	public RescueBrain() {
		victimsAnnihilator = new VictimsAnnihilator(this);
	}

	private void findExit() {

	}

	void checkEnterSideAndAlign() {
		Log.proc();
		if (uFrontal.distance.raw > 375) {
			Servo.encoder(10);
			Distance saveRight = uRight.distance;
			Servo.encoder(-10);
			Servo.nextAngleLeft(50);
			if (uFrontal.distance.raw < 260 && saveRight.raw < 65) {
				this.currentSide = false;
			} else {
				this.currentSide = true;
			}
		} else {
			if (uFrontal.distance.raw < 260) {
				this.currentSide = true;
			} else {
				this.currentSide = false;
			}
			Servo.nextAngleLeft(50);
		}

		Log.info($"this.currentSide: {this.currentSide}");
	}

	private bool isWall() => uFrontal.distance.raw < 25 || (s1.isRescueExit() || s2.isRescueExit()) || (s1.isRescueEnter() || s2.isRescueEnter());

	private sbyte isWallBack() {
		if (bBack.state.pressed) {
			return 1;
		} else if ((s1.isRescueExit() || s2.isRescueExit()) || (s1.isRescueEnter() || s2.isRescueEnter())) {
			return -1;
		}
		return 0;
	}

	public void rescueRescueKit() {
		if (Actuator.kit) {
			Servo.rotate(45);
			Servo.encoder(22);
			Servo.rotate(-90);
			victimsAnnihilator.rescueDefault();
			Servo.rotate(90);
			if (!this.currentSide) {
				Servo.forward();
				while (!this.isWall()) { }
				Servo.encoder(-6);
				Servo.rotate(45);
			} else {
				Servo.encoder(-20);
				Servo.rotate(-45);
			}
		} else {
			if (!this.currentSide) {
				Servo.rotate(45);
				Servo.forward();
				while (!this.isWall()) { }
				Servo.encoder(-6);
				Servo.rotate(45);
			}
		}
	}

	private void setupMoveActions() {
		if (uFrontal.distance.raw > 120) {
			this.actionMove[0] = () => Servo.forward();
			this.actionMove[1] = () => Servo.backward();
			this.actionMove[2] = () => Servo.nextAngleRight(80);
			this.actionMove[3] = () => Servo.nextAngleLeft(170);
			this.actionMove[4] = () => Servo.rotate(-45);
			this.actionMove[5] = () => Servo.rotate(-135);
		} else {
			this.actionMove[0] = () => Servo.backward();
			this.actionMove[1] = () => Servo.forward();
			this.actionMove[2] = () => Servo.nextAngleLeft(80);
			this.actionMove[3] = () => { };
			this.actionMove[4] = () => Servo.rotate(45);
			this.actionMove[5] = () => Servo.rotate(-45);
		}
	}

	public void findTriangleArea() {
		Log.proc();
		this.checkEnterSideAndAlign();
		Log.debug("Finding triangle");
		while (!s3.isTriangle()) {
			Servo.forward();
			if (this.isWall()) {
				Log.debug("Wall finded");
				Servo.encoder(-2);
				Servo.nextAngleRight(50);
				this.currentSide = !this.currentSide;
				Log.info($"this.currentSide: {this.currentSide}");
				Log.debug("Finding triangle");
			}
		}
		Log.debug("Tringle founded!");

		this.rescueRescueKit();

		this.setupMoveActions();
	}


	public class VictimsAnnihilator {

		byte rescuedVictims = 0;

		RescueRoute.RescueBrain brain;

		public VictimsAnnihilator(RescueRoute.RescueBrain _brain) {
			this.brain = _brain;
		}

		private void exitMain() {
		}

		public void find() {
			Log.debug($"Finding victim... rescuedVictims: {this.rescuedVictims}");
			Servo.alignNextAngle();
			this.brain.actionMove[0]();
			float currentDistance = uRight.distance.raw;
			while (true) {
				currentDistance = uRight.distance.raw;
				if (currentDistance < 230) {
					if (currentDistance < 45) {
						Servo.encoder(12, 150);
						Servo.encoder(-12);
					}
					Servo.stop();
					break;
				}
			}
			this.captureRight(currentDistance);
		}

		private bool captureRight(float distance) {
			Servo.rotate(90);
			Servo.alignNextAngle();
			Actuator.open();
			Actuator.alignDown();
			Servo.forward(200);
			Time.resetTimer();
			int beforeDelay = 0;
			while (true) {
				Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
				if (bc.HasVictim()) {
					break;
				}
				if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
					break;
				}
				if (this.brain.isWall()) {
					beforeDelay = 2048;
					break;
				}
				Time.sleep(32);
			}
			Actuator.close();
			Actuator.angle(0);
			Actuator.position(45);
			Servo.stop();
			Actuator.position(88);
			sbyte tempWallRead;
			Servo.backward();
			Time.sleep(beforeDelay);
			Time.resetTimer();
			while (true) {
				tempWallRead = this.brain.isWallBack();
				if ((tempWallRead == 1) || (Servo.speed() < 0.5f && Time.timer.millis > 256)) {
					Servo.encoder(8);
					break;
				} else if (tempWallRead == -1) {
					Servo.encoder(26);
					break;
				}
			}
			if (Actuator.victim) {
				if (Temperature.victimAlive || this.rescuedVictims == 2) {
					Log.debug("Live victim or dead can be rescue");
					this.brain.actionMove[2]();

					rescueFast();
					this.rescuedVictims++;
					if (this.rescuedVictims == 3) {
						Servo.stop();
						Log.clear();
						this.exitMain();
					} else if (this.rescuedVictims == 2) {
						Servo.stop();
						Log.clear();

						if (uRight.distance.raw > 50) {
							this.brain.actionMove[4]();
							Actuator.open();
							Actuator.alignDown();
							beforeDelay = 0;
							Servo.forward(180);
							while (true) {
								Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
								if (bc.HasVictim()) {
									break;
								}
								if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
									break;
								}
								if (this.brain.isWall()) {
									beforeDelay = 1024;
									break;
								}
								Time.sleep(32);
							}
							Actuator.close();
							Actuator.angle(0);
							Actuator.position(45);
							Servo.stop();
							Actuator.position(88); ;
							Servo.rotate(-4);
							Servo.rotate(-90);
							this.rescueDefault();
						} else {
							this.brain.actionMove[4]();
							Actuator.open();
							Actuator.alignDown();
							beforeDelay = 0;
							Servo.forward(180);
							while (true) {
								Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
								if (bc.HasVictim()) {
									break;
								}
								if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
									break;
								}
								if (this.brain.isWall()) {
									beforeDelay = 1024;
									break;
								}
								Time.sleep(32);
							}
							Actuator.close();
							Actuator.angle(0);
							Actuator.position(45);
							Servo.stop();
							Actuator.position(88); ;
							Servo.rotate(-4);
							Servo.rotate(90);
							this.rescueDefault();
						}

						this.exitMain();
					}
					this.brain.actionMove[3]();

					return true;
				} else {
					Log.debug("Dead victim, go to queue");
					this.brain.actionMove[2]();
					Servo.forward();
					while (!s3.isTriangle()) { }
					Servo.stop();
					this.brain.actionMove[4]();
					Servo.encoder(4, 200);
					Actuator.alignDown();
					Servo.encoder(-4, 200);
					Actuator.alignUp();
					this.brain.actionMove[5]();
					return true;
				}
			} else {
				return false;
			}

		}

		private void alignRescue() {

		}

		public void rescueFast() {
			if (uFrontal.distance.raw < 140) {
				Servo.encoder(-8);
			}
			Servo.forward();
			Actuator.position(25);
			while ((Servo.speed() > 0.8f || Time.timer.millis < 256)) { }
			Time.sleep(312);
			Servo.backward();
			Time.sleep(96);
			Servo.forward();
			Time.sleep(96);
			Servo.stop();
			Time.sleep(128);
			if (Actuator.victim) {
				Servo.encoder(-10);
				Servo.forward();
				while ((Servo.speed() > 0.8f || Time.timer.millis < 256)) { }
				Time.sleep(312);
				Servo.backward();
				Time.sleep(96);
				Servo.forward();
				Time.sleep(96);
				Servo.stop();
			}

			Actuator.alignUp();
		}

		public void rescueDefault() {
			Log.proc();
			Servo.forward(200);
			Time.sleep(64);
			Actuator.open();
			Actuator.dropVictim();
			for (int i = 0; i < 2; i++) {
				Servo.backward();
				Time.sleep(64);
				Servo.forward();
				Time.sleep(64);
			}
			if (Actuator.victim) {
				for (int i = 0; i < 2; i++) {
					Servo.backward();
					Time.sleep(64);
					Servo.forward();
					Time.sleep(64);
				}
			}
			Servo.stop();
			Actuator.close();
			Actuator.alignUp();
		}
	}
}
