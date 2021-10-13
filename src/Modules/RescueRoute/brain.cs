
public class RescueBrain {

	public VictimsAnnihilator victimsAnnihilator;
	public Degrees defaultEnterDegrees;
	public bool currentSide;

	public ActionHandler[] actionMove = new ActionHandler[6];

	public sbyte sideMultiplier = 0;

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

	private bool isWallWithoutExit() => uFrontal.distance.raw < 25 || (s1.isRescueEnter() || s2.isRescueEnter());

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
			this.sideMultiplier = 1;
			this.actionMove[0] = () => Servo.forward();
			this.actionMove[1] = () => Servo.backward();
			this.actionMove[2] = () => { Servo.rotate(90); Servo.alignNextAngle(); };
			this.actionMove[3] = () => { Servo.rotate(-180); Servo.alignNextAngle(); };
			this.actionMove[4] = () => Servo.rotate(-45);
			this.actionMove[5] = () => Servo.rotate(-135);
		} else {
			this.sideMultiplier = -1;
			this.actionMove[0] = () => Servo.backward();
			this.actionMove[1] = () => Servo.forward();
			this.actionMove[2] = () => { Servo.rotate(-90); Servo.alignNextAngle(); };
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

	private void findLineAfterExit(ActionHandler direction, ActionHandler fix, int maxDegrees) {
		Degrees defaultAxis = Gyroscope.x;

		Degrees max = new Degrees(defaultAxis.raw + maxDegrees);

		bool findLineBase(Degrees degrees) {
			while (!(Gyroscope.x % degrees)) {
				if (FloorRoute.CrossPath.checkLine(mainFollow)) {
					Servo.stop();
					return true;
				}
			}
			Servo.stop();
			return false;
		}


		direction();
		if (!findLineBase(max)) {
			fix();
		}
	}

	public void findExitArea() {
		Log.proc();
		Log.debug("Finding exit");

		byte counter = 0;
		while (true) {
			Servo.forward();
			if (this.isWallWithoutExit()) {
				counter = 0;
				Log.debug("Wall finded");
				Servo.encoder(-2);
				Servo.nextAngleLeft(80);
				Log.debug("Finding exit");
			}

			if (s1.isRescueExit() || s2.isRescueExit()) {
				counter = 0;
				Servo.stop();
				Servo.rotate(-30);
				Servo.encoder(8);
				Servo.rotate(15);
				this.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 35);
				break;
			}

			if (uRight.distance.raw > 40) {
				Log.info($"Last uRight read: {uRight.distance.raw}");
				counter++;
			} else {
				counter = 0;
			}

			if (counter > 8) {
				if (Time.timer.millis < (1024 - 256)) {
					continue;
				}
				counter = 0;
				Servo.stop();
				Log.debug("Finded hole!, verifying...");
				Servo.rotate(60);
				Time.resetTimer();
				while (true) {
					Servo.forward();
					if (s1.isRescueEnter() || s2.isRescueEnter()) {
						Log.debug("Enter hole!");
						Servo.stop();
						Servo.backward(280);
						Time.sleep(Time.timer);
						Servo.stop();
						Servo.rotate(-60);
						Servo.forward();
						Time.sleep(256);
						Servo.stop();
						Time.resetTimer();
						break;
					}
					if (s1.isRescueExit() || s2.isRescueExit()) {
						Log.debug("Exit hole!");
						Servo.stop();
						Servo.encoder(8);
						Servo.rotate(15);
						this.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 50);
						return;
					}
				}
			}
			Time.sleep(16);
		}
	}

	public class VictimsAnnihilator {

		byte rescuedVictims = 0;

		bool rescuedDeadVictim = false;

		RescueRoute.RescueBrain brain;

		public VictimsAnnihilator(RescueRoute.RescueBrain _brain) {
			this.brain = _brain;
		}

		private void exitMain(bool forceSide = false, bool fixRotate = true) {
			Log.proc();
			void mainFollowAfterRescue() {
				for (; ; ) {
					mainFollow.proc();
					mainRescue.verify();
				}
			}

			Log.info($"Exiting with forceSide: {forceSide}, fixRotate: {fixRotate}");

			if (fixRotate) {
				Servo.nextAngleLeftDiagonal(80);
			}

			if (!forceSide) {
				Time.resetTimer();
				while (true) {
					Servo.forward();
					if (this.brain.isWallWithoutExit() || (Servo.speed() < 0.5f && Time.timer.millis > 256)) {
						Log.debug("Wall finded");
						Servo.stop();
						Servo.encoder(-2);
						Servo.rotate(-45);
						Servo.alignNextAngle();
						break;
					}

					if ((s1.isRescueExit() || s2.isRescueExit())) {
						Servo.stop();
						Servo.encoder(7);
						Servo.rotate(15);
						this.brain.findLineAfterExit(() => Servo.right(), () => Servo.nextAngleLeft(20), 35);
						mainFollowAfterRescue();
					}
				}
			}

			this.brain.findExitArea();

			mainFollowAfterRescue();
		}

		public void find() {
			Log.debug($"Finding victim... rescuedVictims: {this.rescuedVictims}");
			Servo.alignNextAngle();
			this.brain.actionMove[0]();
			float currentDistance = uRight.distance.raw;
			while (true) {
				currentDistance = uRight.distance.raw;
				if (currentDistance < 230) {
					Servo.stop();
					break;
				}
			}
			this.captureRight(currentDistance);
		}

		private bool captureRight(float distance) {
			if (distance < 170) {
				if (this.brain.currentSide) {
					Servo.encoder(-1.7f);
				} else {
					Servo.encoder(1.7f);
				}
			}
			Servo.rotate(90);
			Servo.alignNextAngle();
			if (distance < 45) {
				Servo.encoder(12, 150);
				Servo.encoder(-12);
			}
			Actuator.open();
			Actuator.alignDown();
			Servo.forward(200);
			Time.resetTimer();
			int beforeDelay = 1024;
			while (true) {
				Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
				if (bc.HasVictim()) {
					break;
				}
				if (Servo.speed() < 1f && Time.timer.millis > 256) {
					break;
				}

				if (uFrontal.distance.raw <= 45) {
					Time.sleep(32);
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
			Servo.encoder(1);
			Time.sleep(128);
			Servo.encoder(-1);
			sbyte tempWallRead;
			Servo.backward();
			Time.sleep(beforeDelay);
			Time.resetTimer();
			while (true) {
				tempWallRead = this.brain.isWallBack();
				if ((tempWallRead == 1) || (Servo.speed() < 1f && Time.timer.millis > 256)) {
					Servo.encoder(8);
					break;
				} else if (tempWallRead == -1) {
					Servo.encoder(25);
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
						Log.debug("Rescued 3 victims");
						if (this.brain.currentSide) {
							Servo.rotate(-180);
							Servo.alignNextAngle();
						}
						if (uRight.distance.raw < 40 && !this.brain.currentSide) {
							Servo.rotate(-45);
							this.exitMain(false, false);
						}
						this.exitMain(true, false);

					} else if (this.rescuedVictims == 2 && this.rescuedDeadVictim) {
						Servo.stop();
						Log.clear();

						this.brain.actionMove[4]();
						Servo.encoder(-3);
						Actuator.open();
						Actuator.alignDown();
						beforeDelay = 0;
						Servo.forward(180);
						Time.resetTimer();
						while (true) {
							Log.debug($"Actuator.victim: {bc.HasVictim()}, Servo.speed: {Servo.speed()}");
							if (bc.HasVictim()) {
								break;
							}
							if (Servo.speed() < 0.5f && Time.timer.millis > 412) {
								break;
							}
							if (this.brain.isWall()) {
								beforeDelay = 1024;
								break;
							}
							Time.sleep(32);
						}
						Actuator.close();
						Time.sleep(128);
						Actuator.angle(0);
						Actuator.position(45);
						Servo.stop();
						Actuator.position(88);
						Servo.forward();
						Time.resetTimer();
						while (true) {
							if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
								break;
							}

							if (this.brain.isWall()) {
								break;
							}
						}
						Servo.encoder(-30);
						Servo.rotate(this.brain.sideMultiplier * 90);
						this.rescueDefault();

						this.exitMain();
					}
					this.brain.actionMove[3]();

					return true;
				} else if (this.rescuedVictims == 2 && !this.rescuedDeadVictim) {
					this.brain.actionMove[3]();
					return false;
				} else {
					Log.debug("Dead victim, go to queue");
					this.rescuedDeadVictim = true;
					this.brain.actionMove[2]();
					Servo.forward();
					while (!s3.isTriangle()) { }
					Servo.stop();
					this.brain.actionMove[4]();
					Servo.forward();
					Time.resetTimer();
					while (true) {
						if (Servo.speed() < 0.5f && Time.timer.millis > 256) {
							break;
						}

						if (this.brain.isWall()) {
							break;
						}
					}
					Servo.encoder(-27);
					Time.sleep(128);
					for (int i = 0; i < 10; i++) {
						Actuator.position(88 - (i * 8.8f));
						Time.sleep(128);
					}
					Servo.encoder(-18);
					Actuator.alignUp();
					this.brain.actionMove[5]();
					return true;
				}
			} else {
				this.brain.actionMove[2]();
				return false;
			}

		}

		public void rescueFast() {
			if (uFrontal.distance.raw < 140) {
				Servo.encoder(-8);
			}
			Servo.forward();
			Actuator.position(25);
			while ((Servo.speed() > 2f || Time.timer.millis < 256)) { }
			Time.sleep(312);
			Actuator.angle(10);
			Servo.backward();
			Time.sleep(128);
			Servo.forward();
			Time.sleep(128);
			Servo.stop();
			Time.sleep(128);
			if (Actuator.victim) {
				Servo.encoder(-10);
				Servo.forward();
				while ((Servo.speed() > 0.8f || Time.timer.millis < 256)) { }
				Time.sleep(312);
				Servo.backward();
				Time.sleep(128);
				Servo.forward();
				Time.sleep(128);
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
				Servo.stop();
				Time.sleep(64);
			}
			if (Actuator.victim) {
				for (int i = 0; i < 2; i++) {
					Servo.backward();
					Time.sleep(64);
					Servo.forward();
					Time.sleep(64);
					Servo.stop();
					Time.sleep(128);
				}
			}
			Servo.stop();
			Actuator.close();
			Actuator.alignUp();
		}
	}
}
