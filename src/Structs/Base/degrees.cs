public struct Degrees{
	public Degrees(float raw_){
		this.raw = raw_;
	}

	public float raw {
		get{
			return this.raw;
		}
		set{
			this.raw = (value + 360) % 360;
		}
	}

	//Basic operators
	public static bool operator >(Degrees a, Degrees b) => a.raw > b.raw;
	public static bool operator <(Degrees a, Degrees b) => a.raw < b.raw;
	public static bool operator >=(Degrees a, Degrees b) => a.raw >= b.raw;
	public static bool operator <=(Degrees a, Degrees b) => a.raw <= b.raw;
	public static bool operator ==(Degrees a, Degrees b) => a.raw == b.raw;
	public static bool operator !=(Degrees a, Degrees b) => a.raw != b.raw;
	public static float operator -(Degrees a, Degrees b) => ((a.raw - b.raw) + 360) % 360;
	public static float operator +(Degrees a, Degrees b) => ((a.raw + b.raw) + 360) % 360;
	public static float operator *(Degrees a, Degrees b) => ((a.raw * b.raw) + 360) % 360;
	public static float operator /(Degrees a, Degrees b) => ((a.raw / b.raw) + 360) % 360;
}
