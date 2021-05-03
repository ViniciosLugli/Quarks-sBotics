import("Structs/Base/degrees")

class Gyroscope{
	public static Degrees x {
		get{
			return new Degrees(bc.Compass());
		}
	}
	public static Degrees z {
		get{
			return new Degrees(bc.Inclination());
		}
	}

	public static void NOP(){
		Log.clear();
		Log.proc("Gyroscope | NOP()");
		bc.Compass();
		bc.Inclination();
	}
}
