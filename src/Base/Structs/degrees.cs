public struct Degrees{
	public Degrees(float raw_){
		this.raw = (raw_ + 360) % 360;
	}

	public float raw;

	//Basic operators
	public static bool operator >(Degrees a, Degrees b) => a.raw > b.raw;
	public static bool operator <(Degrees a, Degrees b) => a.raw < b.raw;
	public static bool operator >=(Degrees a, Degrees b) => a.raw >= b.raw;
	public static bool operator <=(Degrees a, Degrees b) => a.raw <= b.raw;
	public static float operator -(Degrees a, Degrees b) => ((a.raw - b.raw) + 360) % 360;
	public static float operator +(Degrees a, Degrees b) => ((a.raw + b.raw) + 360) % 360;
	public static float operator *(Degrees a, Degrees b) => ((a.raw * b.raw) + 360) % 360;
	public static float operator /(Degrees a, Degrees b) => ((a.raw / b.raw) + 360) % 360;
	public static bool operator %(Degrees a, Degrees b) => (a.raw+1 > b.raw) && (a.raw-1 < b.raw);
}

private struct DegreesRange{
	public DegreesRange(float min_, float max_){
		this.min = new Degrees(min_);
		this.max = new Degrees(max_);
	}
	public Degrees min, max;

	public bool isOnRange(Degrees currentGyro) => (currentGyro > this.min) && (currentGyro < this.max);
}
