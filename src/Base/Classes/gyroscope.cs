import("Base/Structs/degrees.cs");

public static class Gyroscope {

	public static Degrees[] points = new Degrees[] { new Degrees(359), new Degrees(0), new Degrees(90), new Degrees(180), new Degrees(270) };
	public static Degrees[] diagonals = new Degrees[] { new Degrees(45), new Degrees(135), new Degrees(225), new Degrees(315) };

	public static Degrees x {
		get => new Degrees((float)bc.Compass());
	}
	public static Degrees z {
		get => new Degrees((float)bc.Inclination());
	}

	public static bool inPoint(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees point in Gyroscope.points) {
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)) {
					return true;
				}
			}
			return false;
		} else {
			foreach (Degrees point in Gyroscope.points) {
				if (Gyroscope.x % point) {
					return true;
				}
			}
			return false;
		}
	}

	public static bool inDiagonal(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (((Gyroscope.x.raw + offset) >= diagonal.raw) && (Gyroscope.x.raw - offset <= diagonal.raw)) {
					return true;
				}
			}
			return false;
		} else {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (Gyroscope.x % diagonal) {
					return true;
				}
			}
			return false;
		}
	}

	public static float? inRawPoint(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees point in Gyroscope.points) {
				if (((Gyroscope.x.raw + offset) >= point.raw) && (Gyroscope.x.raw - offset <= point.raw)) {
					return point.raw;
				}
			}
		} else {
			foreach (Degrees point in Gyroscope.points) {
				if (Gyroscope.x % point) {
					return point.raw;
				}
			}
		}
		return null;
	}

	public static float? inRawDiagonal(bool angExpand = true, float offset = 8) {
		if (angExpand) {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (((Gyroscope.x.raw + offset) >= diagonal.raw) && (Gyroscope.x.raw - offset <= diagonal.raw)) {
					return diagonal.raw;
				}
			}
		} else {
			foreach (Degrees diagonal in Gyroscope.diagonals) {
				if (Gyroscope.x % diagonal) {
					return diagonal.raw;
				}
			}
		}
		return null;
	}

	public static bool isLifted() => Gyroscope.z.raw > 300 && Gyroscope.z.raw < 358;

	public static void NOP() {
		Log.clear();
		Log.proc();
		bc.Compass();
		bc.Inclination();
	}
}
