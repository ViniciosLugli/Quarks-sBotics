import("Base/Structs/clock")

class Time{
	public static Clock current {
		get {
			return new Clock(bc.Millis());
		}
	}

	public Clock timer {
		get {
			return new Clock(bc.Timer());
		}
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);

	public static void sleep(Clock clock) => bc.Wait(clock.millis);
};
