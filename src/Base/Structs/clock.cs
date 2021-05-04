public struct Clock{
	public Clock(int millis_){
		this.millis = millis_;
	}

	// public uint sec
	public int seconds {
			get{
				return (int)(this.millis/1000);
			}
		}
	public int millis;
	public uint micros {
			get{
				return (uint)(this.millis*1000);
			}
		}

	//Basic operators
	public static bool operator >(Clock a, Clock b) => a.millis > b.millis;
	public static bool operator <(Clock a, Clock b) => a.millis < b.millis;
	public static bool operator >=(Clock a, Clock b) => a.millis >= b.millis;
	public static bool operator <=(Clock a, Clock b) => a.millis <= b.millis;
	public static bool operator ==(Clock a, Clock b) => a.millis == b.millis;
	public static bool operator !=(Clock a, Clock b) => a.millis != b.millis;
	public static int operator -(Clock a, Clock b) => a.millis - b.millis;
	public static int operator +(Clock a, Clock b) => a.millis + b.millis;
	public static int operator *(Clock a, Clock b) => a.millis * b.millis;
	public static int operator /(Clock a, Clock b) => a.millis / b.millis;
}
