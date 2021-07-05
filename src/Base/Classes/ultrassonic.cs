import("Base/Structs/distance.cs");

class Ultrassonic{
	private byte SensorIndex = 0;

	public Ultrassonic(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Distance distance{
		get => new Distance(bc.Distance((int)this.SensorIndex));
	}

	public void NOP(){
		Log.clear();
		Log.proc();
		bc.Distance((int)this.SensorIndex);
	}
}
