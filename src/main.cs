//Base for robot and utils
import("globalVariables.cs");
import("Base/Types/basics.cs");
import("Utils/calc.cs");
import("Utils/consoleColors.cs");
import("Base/Classes/console.cs");
import("Base/Classes/robot.cs");
import("Base/Classes/time.cs");
import("Base/Classes/button.cs");
import("Base/Classes/led.cs");
import("Base/Classes/buzzer.cs");
import("Base/Classes/pencil.cs");
import("Base/Classes/temperature.cs");
import("Base/Classes/gyroscope.cs");
import("Base/Classes/reflective.cs");
import("Base/Classes/ultrassonic.cs");
import("Base/Classes/actuator.cs");
import("Base/Classes/servo.cs");
import("Base/Structs/vector2.cs");
import("Base/Attributes/debug.cs");


//Modules for competition challenges
//import("Modules/AI/ai.cs");
import("Modules/FloorRoute/floorRoute.cs");
import("Modules/RescueRoute/rescueRoute.cs");

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);
static DegreesRange floor = new DegreesRange(355, 5);

static Reflective s1 = new Reflective(1), s2 = new Reflective(0), s3 = new Reflective(2);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1);

static Button bBack = new Button(0);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 190);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 26);
static RescueRoute mainRescue = new RescueRoute();

//----------------------------- Main ---------------------------------------//
//List<double[]> INPUT = new List<double[]>();
//List<double> OUTPUT = new List<double>();

#if (false) //DEBUG MODE MAIN ------------------------------------

void testRefreshRate() {
	long currentTime = Time.currentUnparsed;
	float pval = s1.light.raw;
	float after = s1.light.raw;
	Servo.right();
	while (pval == after) {
		after = s1.light.raw;
	}
	Log.debug($"Time: {Time.currentUnparsed - currentTime}");
	Log.info($"values: {pval} -> {after}");
	Servo.stop();
	Time.sleep(16);
}


void testTurnSpeed() {
	long currentTime = Time.currentUnparsed;
	Servo.rotate(90);
	Log.debug($"Time: {Time.currentUnparsed - currentTime}");
	Time.sleep(16);
}

void showPorcentRGB() {
	Log.info($"s3: {s3.rgb.showPorcentRGB()}");
}

void Main() {
	Servo.backward();
	for (; ; ) {
		Log.debug(Servo.speed());
		//showPorcentRGB();
	}
}

#else //DEFAULT MAIN ------------------------------------------------------------------------------------

//Setup program
void setup() {
	//Actuator.alignUp();
	bc.ActuatorSpeed(150);
	bc.ActuatorUp(600);
	Time.resetTimer();
}

//Main loop
void loop() {
	mainFollow.proc();
	mainRescue.verify();
}

void Main() {
	setup();
	for (; ; ) {
		loop();
	}
}

#endif
