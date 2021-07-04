import("Base/Structs/action.cs");

class Button{
	private byte SensorIndex = 1;

	public Button(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Action state{
		get => new Action(bc.Touch((int)this.SensorIndex));
	}

	public void NOP(){
		Log.clear();
		Log.proc($"Button({SensorIndex})", "NOP()");
		bc.Touch((int)this.SensorIndex);
	}
}
