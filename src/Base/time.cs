class Time{
	public static int millis(){
		return bc.Millis();
	}

	public static int timer(){
		return bc.Timer();
	}

	public static void resetTimer(){
		bc.ResetTimer();
	}

	public static void sleep(int ms){
		bc.Wait(ms);
	}

	public static bool await(object tester){
		return tester();
	}
};
