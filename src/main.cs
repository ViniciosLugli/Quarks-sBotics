//Base for robot and utils
import("variables.cs");
import("Utils/calc.cs");
import("Utils/consoleColors.cs");
import("Base/Classes/console.cs");
import("Base/Classes/time.cs");
import("Base/Classes/button.cs");
import("Base/Classes/buzzer.cs");
import("Base/Classes/pencil.cs");
import("Base/Classes/temperature.cs");
import("Base/Classes/gyroscope.cs");
import("Base/Classes/reflective.cs");
import("Base/Classes/ultrassonic.cs");
import("Base/Classes/actuator.cs");
import("Base/Classes/servo.cs");


//Modules for competition challenges
import("Modules/position.cs");
import("Modules/green.cs");
import("Modules/crosspath.cs");
import("Modules/followline.cs");

/* --------------- General code --------------- */

//Instance sensors ---------------------------------------------
static Reflective s1 = new Reflective(3);
static Reflective s2 = new Reflective(2);
static Reflective s3 = new Reflective(1);
static Reflective s4 = new Reflective(0);

//Instance modules ---------------------------------------------

FollowLine mainFollower = new FollowLine(13f);

//Setup program
void setup(){
	Actuator.alignUp();
}

//Main loop
void loop(){
	if((CurrentState & (byte)States.FOLLOWLINE) != 0){
		mainFollower.proc(160);
		Green.verify();
		CrossPath.verify();
	} else if ((CurrentState & (byte)States.OBSTACLE) != 0){

	}else if ((CurrentState & (byte)States.UPRAMP) != 0){

	}else if ((CurrentState & (byte)States.DOWNRAMP) != 0){

	}else if ((CurrentState & (byte)States.RESCUERAMP) != 0){

	}else if ((CurrentState & (byte)States.RESCUE) != 0){

	}else if ((CurrentState & (byte)States.RESCUEEXIT) != 0){

	}else if ((CurrentState & (byte)States.NOP) != 0){
		//Formater.format(teste);
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
