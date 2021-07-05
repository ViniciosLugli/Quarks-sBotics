import("Base/Structs/colors.cs");

public class Reflective{
	private byte SensorIndex = 0;

	public Reflective(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Light light{
		get => new Light(bc.Lightness((int)this.SensorIndex));
	}

	public Color rgb{
		get => new Color(
				bc.ReturnRed((int)this.SensorIndex),
				bc.ReturnGreen((int)this.SensorIndex),
				bc.ReturnBlue((int)this.SensorIndex)
			);
	}
	public bool hasLine() => bc.ReturnRed((int)this.SensorIndex) < 26;

	public bool hasGreen(){
			float rgb = this.rgb.r + this.rgb.g + this.rgb.b;
			byte pR = (byte)Calc.map(this.rgb.r, 0, rgb, 0, 100);
			byte pG = (byte)Calc.map(this.rgb.g, 0, rgb, 0, 100);
			byte pB = (byte)Calc.map(this.rgb.b, 0, rgb, 0, 100);
			return ((pG > pR) && (pG > pB) && (pG > 65));
		}

	public void NOP(){
		Log.clear();
		Log.proc();
		bc.Lightness((int)this.SensorIndex);
		bc.ReturnRed((int)this.SensorIndex);
		bc.ReturnGreen((int)this.SensorIndex);
		bc.ReturnBlue((int)this.SensorIndex);
	}
}
