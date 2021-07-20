import("Base/Structs/degrees.cs");

public static class Gyroscope{

	private static Degrees[] points = new Degrees[] {new Degrees(359),new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(){
		foreach(Degrees point in Gyroscope.points){
			if(((Gyroscope.x.raw + 8) > point.raw) && (Gyroscope.x.raw - 8 < point.raw)){
				return true;
			}
		}
		return false;
	}

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
