//Base for robot and utils
import("Utils/calc")
import("Base/Classes/console")
import("Base/Classes/time")
import("Base/Classes/button")
import("Base/Classes/buzzer")
import("Base/Classes/pencil")
import("Base/Classes/temperature")
import("Base/Classes/gyroscope")
import("Base/Classes/reflective")
import("Base/Classes/ultrassonic")
import("Base/Classes/actuator")
import("Base/Classes/servo")



//Modules for competition challenges
import("Modules/position")
import("Modules/green")
import("Modules/crosspath")
import("Modules/followline")


//General code:

//Instance sensors ---------------------------------------------
static Reflective s1 = new Reflective(3);
static Reflective s2 = new Reflective(2);
static Reflective s3 = new Reflective(1);
static Reflective s4 = new Reflective(0);

//Instance modules ---------------------------------------------
FollowLine mainFollower = new FollowLine(13f);

void setup(){
	Actuator.alignUp();
}

void loop(){
	if((Gyroscope.z.raw >= 330) && (Gyroscope.z.raw <= 355)){
		mainFollower.proc(210);
	}else{
		mainFollower.proc(160);
	}

	Green.verify();
	CrossPath.verify();
}

//----------------------------- Main ---------------------------------------//
void Main(){
	setup();
	for(;;){
		loop();
	}
}
