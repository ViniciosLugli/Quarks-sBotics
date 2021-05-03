import("Structs/Base/celsius")

class Temperature{
	public static Celsius celsius {
		get{
			return new Celsius((float)bc.Heat());
		}
	}

	public static void NOP(){
		Log.clear();
		Log.proc("Temperature | NOP()");
		bc.Heat();
	}
}
