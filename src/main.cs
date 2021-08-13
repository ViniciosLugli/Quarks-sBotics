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

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, 145);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 26);
static RescueRoute mainRescue = new RescueRoute(ref s1, ref s2, 180);

//----------------------------- Main ---------------------------------------//
List<double[]> INPUT = new List<double[]>();
List<double> OUTPUT = new List<double>();

#if (false) //DEBUG MODE MAIN

//Setup debug program
void setup() {
	AI.Analyzer.setup();
}

//Main loop debug
void loop() {
	var tempResult = AI.Trainner.reflectives(0);
	INPUT.Add(tempResult.input);
	OUTPUT.Add(tempResult.output);
	Time.sleep(48);
}

string parseArray1D(double[][] arr) {
	var s = new System.Text.StringBuilder();
	foreach (var info in arr) {
		s.Append("{");
		foreach (var item in info) {
			s.Append($"{item.ToString().Replace(',', '.')}, ");
		}
		s.Remove(s.Length - 2, 2);
		s.Append("}, ");
	}
	s.Remove(s.Length - 2, 2);
	return s.ToString();
}

void Main() {
	setup();

	//AI.Controller.train(
	//	new double[,] {
	//		import("Variables/input_train")
	//	},
	//	new double[,] {
	//		import("Variables/output_train")
	//	},
	//	new double[,] { { 0, 0, 1 } },
	//	5000
	//);


	for (int i = 0; i <= 512; i++) {
		Log.debug(i);
		loop();
	}

	AI.Result.export(parseArray1D(INPUT.ToArray()), "input_train");
	AI.Result.export("{" + String.Join(", ", OUTPUT.ToArray()) + "}", "output_train");
}

#else //DEFAULT MAIN

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

void Main() {
	setup();
	mainRescue.check();
	for (; ; ) {
		loop();
	}
}

#endif
