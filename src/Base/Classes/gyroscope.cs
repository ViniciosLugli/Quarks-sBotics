import("Base/Structs/degrees.cs");

public static class Gyroscope{
	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
