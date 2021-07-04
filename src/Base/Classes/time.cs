import("Base/Structs/clock.cs");

public static class Time{
	public static Clock current {
		get => new Clock(bc.Millis());
	}

	static public Clock timer {
		get => new Clock(bc.Timer());
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);
	public static void sleep(Clock clock) => bc.Wait(clock.millis);
};
