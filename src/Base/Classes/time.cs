import("Base/Structs/clock.cs");

public static class Time {
	public static Clock current {
		get => new Clock(bc.Millis());
	}

	static public Clock timer {
		get => new Clock(bc.Timer());
	}
	public static long currentUnparsed {
		get => DateTimeOffset.Now.ToUnixTimeMilliseconds();
	}

	public static void resetTimer() => bc.ResetTimer();

	public static void sleep(int ms) => bc.Wait(ms);
	public static void sleep(int ms, ActionHandler callwhile) {
		int toWait = Time.current.millis + ms;
		while (Time.current.millis < toWait) { callwhile(); }
	}
	public static void sleep(Clock clock) => bc.Wait(clock.millis);

	public static void debug() => bc.Wait(123456789);

	public static void skipFrame() => bc.Wait(17);

	public static string date {
		get => string.Format("{0:HH:mm:ss.fff}", DateTime.Now);
	}
};
