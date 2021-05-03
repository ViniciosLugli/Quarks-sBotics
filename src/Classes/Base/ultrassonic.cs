import("Structs/Base/distance")

class Ultrassonic{
	private byte SensorIndex = 0;

	public Ultrassonic(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Distance distance{
		get{
			return new Distance(bc.Distance((int)this.SensorIndex));
		}
	}

	public void NOP(){
		Log.clear();
		Log.proc($"Actuator | NOP()");
		bc.Distance((int)this.SensorIndex);
	}
}
