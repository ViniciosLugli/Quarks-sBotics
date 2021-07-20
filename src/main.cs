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
import("Base/Attributes/debug.cs");


//Modules for competition challenges
import("Modules/FloorRoute/floorRoute.cs");

/* --------------- General code --------------- */

//Instances ---------------------------------------------
static DegreesRange upRamp = new DegreesRange(330, 355);
static DegreesRange downRamp = new DegreesRange(5, 30);

static Reflective s1 = new Reflective(3), s2 = new Reflective(2), s3 = new Reflective(1), s4 = new Reflective(0);
static FloorRoute.FollowLine mainFollow = new FloorRoute.FollowLine(ref s1, ref s2, ref s3, ref s4, 140);
//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
	Time.resetTimer();
}

//Main loop
void loop(){
	if((this.CurrentState & (byte)States.FOLLOWLINE) != 0){
		mainFollow.proc();
	} else if ((this.CurrentState & (byte)States.OBSTACLE) != 0){

	}else if ((this.CurrentState & (byte)States.UPRAMP) != 0){

	}else if ((this.CurrentState & (byte)States.DOWNRAMP) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUERAMP) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUE) != 0){

	}else if ((this.CurrentState & (byte)States.RESCUEEXIT) != 0){

	}else if ((this.CurrentState & (byte)States.NOP) != 0){
	}
}

//----------------------------- Main ---------------------------------------//

#if(false) //DEBUG MODE MAIN
	void Main(){
		mainFollow.alignSensors();
	for(;;){
	}
	}
#else //DEFAULT MAIN
	void Main(){
		setup();
		for(;;){
			loop();
		}
	}
#endif
