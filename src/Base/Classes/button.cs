import("Base/Structs/action.cs");

public class Button {
	private byte SensorIndex = 1;

	public Button(byte SensorIndex_) {
		this.SensorIndex = SensorIndex_;
	}

	public Action state {
		get => new Action(bc.Touch((int)this.SensorIndex));
	}

	public void NOP() {
		Log.clear();
		Log.proc();
		bc.Touch((int)this.SensorIndex);
	}
}
