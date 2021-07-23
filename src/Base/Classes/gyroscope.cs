import("Base/Structs/degrees.cs");

public static class Gyroscope{

	public static Degrees[] points = new Degrees[] {new Degrees(359),new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270)};

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(bool angExpand = true, byte offset = 8){
		if(angExpand){
			foreach (Degrees point in Gyroscope.points){
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)){
					return true;
				}
			}
			return false;
		}else{
			foreach (Degrees point in Gyroscope.points){
				if (Gyroscope.x % point){
					return true;
				}
			}
			return false;
		}
	}

	public static void NOP(){
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
