static enum EventsType: byte{
	//System, start=0
	Init = 0,
	EndSucess = 1,
	EndErro = 2,
	//Follow line, start=100

	//Rescue, start=200
	RampFound = 200,
	RampInProgress = 201,
	RampCompleted = 202
}

static struct Event{
	uint millis;
	sbyte type;
	string data;
}

class Pipeline{
	private byte robotIndex;

	public Pipeline(sbyte robotIndex){

		this.robotIndex = robotIndex;
	}

	public void init(){

	}

	private appendEvent(Event event){

	}

	private void clearEvents(){
		bc.EraseConsoleFile()
	}

}

void Main(){

}
