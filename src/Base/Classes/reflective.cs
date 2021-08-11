import("Base/Structs/colors.cs");

public class Reflective {
	private byte SensorIndex = 0;

	public Reflective(byte SensorIndex_) {
		this.SensorIndex = SensorIndex_;
	}

	public Light light {
		get => new Light(bc.Lightness((int)this.SensorIndex));
	}

	public Color rgb {
		get => new Color(
				bc.ReturnRed((int)this.SensorIndex),
				bc.ReturnGreen((int)this.SensorIndex),
				bc.ReturnBlue((int)this.SensorIndex)
			);
	}
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 55 && bc.ReturnGreen((int)this.SensorIndex) < 55;

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 50;

	public bool isColored() {
		for (int i = 0; i < 4; i++) {
			float r = bc.ReturnRed((int)this.SensorIndex);
			float b = bc.ReturnBlue((int)this.SensorIndex);
			if (!((r + 2) > b && (r - 2) < b)) {
				return true;
			}
		}
		return false;
	}

	public void NOP() {
		Log.clear();
		Log.proc();
		bc.Lightness((int)this.SensorIndex);
		bc.ReturnRed((int)this.SensorIndex);
		bc.ReturnGreen((int)this.SensorIndex);
		bc.ReturnBlue((int)this.SensorIndex);
	}
}
