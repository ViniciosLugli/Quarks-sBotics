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
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 50 && bc.ReturnGreen((int)this.SensorIndex) < 50 && bc.Lightness((int)this.SensorIndex) < 52;

	public bool isMat() => bc.ReturnRed((int)this.SensorIndex) > 45 && bc.ReturnBlue((int)this.SensorIndex) < 20 && bc.ReturnGreen((int)this.SensorIndex) < 20;

	public bool isEndLine() => bc.ReturnRed((int)this.SensorIndex) > 60 && bc.ReturnBlue((int)this.SensorIndex) < 24 && bc.ReturnGreen((int)this.SensorIndex) < 24;

	public bool isRescueEnter() => (bc.ReturnBlue((int)this.SensorIndex) > (bc.ReturnRed((int)this.SensorIndex) + 4)) && (bc.ReturnBlue((int)this.SensorIndex) > (bc.ReturnGreen((int)this.SensorIndex) + 4));

	public bool isRescueExit() => (bc.ReturnGreen((int)this.SensorIndex) > (bc.ReturnRed((int)this.SensorIndex) + 4)) && (bc.ReturnGreen((int)this.SensorIndex) > (bc.ReturnBlue((int)this.SensorIndex) + 4));

	public bool isTriangle() => bc.ReturnRed((int)this.SensorIndex) < 5 && bc.ReturnBlue((int)this.SensorIndex) < 5 && bc.ReturnGreen((int)this.SensorIndex) < 5;

	public bool isLiveVictim() => bc.ReturnRed((int)this.SensorIndex) > 55 && bc.ReturnBlue((int)this.SensorIndex) > 55 && bc.ReturnGreen((int)this.SensorIndex) > 16;

	public bool isDeadVictim() => bc.ReturnRed((int)this.SensorIndex) < 16 && bc.ReturnBlue((int)this.SensorIndex) < 16 && bc.ReturnGreen((int)this.SensorIndex) < 16;

	public bool isColored() {
		for (int i = 0; i < 4; i++) {
			float r = bc.ReturnRed((int)this.SensorIndex);
			float b = bc.ReturnBlue((int)this.SensorIndex);
			if (!((r + 3) > b && (r - 3) < b)) {
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
