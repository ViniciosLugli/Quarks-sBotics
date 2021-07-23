import("Base/Structs/clock.cs");

public static class Time{
	public static Clock current {
		get => new Clock(bc.Millis());
	}

	static public Clock timer {
		get => new Clock(bc.Timer());
	}
	public static Clock currentUnparsed {
		get => new Clock((int)(DateTimeOffset.Now.ToUnixTimeMilliseconds() - SETUPTIME));
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);
	public static void sleep(int ms, MethodHandler callwhile) {
		int toWait = Time.current.millis + ms;
		while (Time.current.millis < toWait){callwhile();}
	}
	public static void sleep(Clock clock) => bc.Wait(clock.millis);
};
