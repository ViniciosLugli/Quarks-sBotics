//Base for robot and utils
import("globalVariables.cs");
import("Utils/calc.cs");
import("Utils/consoleColors.cs");
import("Base/Classes/robot.cs");
import("Base/Classes/console.cs");
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


//Modules for competition challenges
import("Modules/floorRoute.cs");

/* --------------- General code --------------- */

//Instance sensors ---------------------------------------------
static Reflective s1 = new Reflective(1);
static Reflective s2 = new Reflective(0);

//Instance modules ---------------------------------------------


//Setup program
void setup(){
	Actuator.alignUp();
}

//Main loop
void loop(){
	if((this.CurrentState & (byte)States.FOLLOWLINE) != 0){
		FloorRoute.FollowLine.proc();
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
