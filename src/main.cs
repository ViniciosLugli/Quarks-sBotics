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
import("Modules/AI/ai.cs");
import("Modules/FloorRoute/floorRoute.cs");
import("Modules/RescueRoute/rescueRoute.cs");

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);
static DegreesRange floor = new DegreesRange(355, 5);

static Reflective s1 = new Reflective(1), s2 = new Reflective(0);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 140);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 26);
static RescueRoute mainRescue = new RescueRoute(ref s1, ref s2, 180);
//Instance modules ---------------------------------------------


//Setup program
void setup() {
	Actuator.alignUp();
	Time.resetTimer();
}

//Main loop
void loop() {
	mainFollow.proc();
	mainObstacle.verify();
	mainRescue.verify();
}

//----------------------------- Main ---------------------------------------//

#if (true) //DEBUG MODE MAIN

void Main() {
	AI.Analyzer.setup();
	AI.Controller.train(
		new double[,] { { 0, 0, -1 }, { 1, 1, 1 }, { 1, 0, 1 }, { 0, 1, 1 } },
		new double[,] { { 0, 1, 1, 0 } },
		new double[,] { { 0, 0, 1 } }
		);
	//for (; ; ) {
	//}
}

#else //DEFAULT MAIN

void Main() {
	setup();
	mainRescue.check();
	for (; ; ) {
		loop();
	}
}

#endif
