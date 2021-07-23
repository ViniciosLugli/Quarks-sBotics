public struct Distance{
	public Distance(float raw_){
		this.raw = raw_;
	}

	public float raw;

	public float toRotations() => this.raw / 2;

	//Basic operators
	public static bool operator >(Distance a, Distance b) => a.raw > b.raw;
	public static bool operator <(Distance a, Distance b) => a.raw < b.raw;
	public static bool operator >=(Distance a, Distance b) => a.raw >= b.raw;
	public static bool operator <=(Distance a, Distance b) => a.raw <= b.raw;
	public static float operator -(Distance a, Distance b) => a.raw - b.raw;
	public static float operator +(Distance a, Distance b) => a.raw + b.raw;
	public static float operator *(Distance a, Distance b) => a.raw * b.raw;
	public static float operator /(Distance a, Distance b) => a.raw / b.raw;
}
