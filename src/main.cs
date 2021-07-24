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
import("Modules/FloorRoute/floorRoute.cs");
import("Modules/RescueRoute/rescueRoute.cs");

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);

static Reflective s1 = new Reflective(3), s2 = new Reflective(2), s3 = new Reflective(1), s4 = new Reflective(0);
static Ultrassonic uFrontal = new Ultrassonic(0), uRight = new Ultrassonic(1), uLeft = new Ultrassonic(2);

static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, ref s3, ref s4, 170);
static FloorRoute.Obstacle mainObstacle = new FloorRoute.Obstacle(ref uFrontal, 15);
static RescueRoute mainRescue = new RescueRoute(ref s1, ref s2, ref s3, ref s4, 300);
//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
	Time.resetTimer();
}

//Main loop
void loop(){
	mainFollow.proc();
	mainObstacle.verify();
	mainRescue.verify();
}

//----------------------------- Main ---------------------------------------//

#if (false) //DEBUG MODE MAIN
	void Main(){
		long a = Time.current.millis;
		float abuble = bc.Lightness(1);
		while (abuble == bc.Lightness(1)) {
			Servo.left();
		}
		bc.Print(Time.current.millis - a);
		Servo.stop();
		for (;;){
		}
	}
#else //DEFAULT MAIN
	void Main(){
		setup();
		mainRescue.check();
		for(;;){
			loop();
		}
	}
#endif
