import("Base/time")
import("Utils/calc")
import("Base/actuator")

bool tester(){
	return true;
}

void Main(){
	if(Time.await(tester)){
		actuator.position(88, 150);
	}
}
