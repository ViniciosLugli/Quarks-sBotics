import("Structs/Base/colors")

class Reflective{
	private byte SensorIndex = 0;

	public Reflective(byte SensorIndex_){
		this.SensorIndex = SensorIndex_;
	}

	public Light light{
		get{
			return new Light(bc.Lightness((int)this.SensorIndex));
		}
	}

	public Color rgb{
		get{
			return new Color(
				bc.ReturnRed((int)this.SensorIndex),
				bc.ReturnGreen((int)this.SensorIndex),
				bc.ReturnBlue((int)this.SensorIndex)
			);
		}
	}

	public void NOP(){
		Log.clear();
		Log.proc($"Reflective({SensorIndex}) | NOP()");
		bc.Lightness((int)this.SensorIndex);
		bc.ReturnRed((int)this.SensorIndex);
		bc.ReturnGreen((int)this.SensorIndex);
		bc.ReturnBlue((int)this.SensorIndex);
	}
}
