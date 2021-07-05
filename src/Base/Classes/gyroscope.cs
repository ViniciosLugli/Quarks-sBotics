import("Base/Structs/degrees.cs");

public static class Gyroscope{
	private static List<Degrees> UP_RAMP = new List<Degrees>(){new Degrees(330), new Degrees(355)};
	private static List<Degrees> DOWN_RAMP = new List<Degrees>(){new Degrees(5), new Degrees(30)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool isUpRamp() => (Gyroscope.z >= UP_RAMP[0]) && (Gyroscope.z <= UP_RAMP[1]);
	public static bool isDownRamp() => (Gyroscope.z >= DOWN_RAMP[0]) && (Gyroscope.z <= DOWN_RAMP[1]);

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
