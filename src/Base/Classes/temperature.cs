import("Base/Structs/celsius.cs");

public static class Temperature{
	public static Celsius celsius {
		get => new Celsius((float)bc.Heat());
	}

	public static void NOP(){
		Log.clear();
		Log.proc("Temperature", "NOP()");
		bc.Heat();
	}
}
