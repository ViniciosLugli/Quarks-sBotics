class Led{
	public static void on(byte r , byte g, byte b) => bc.TurnLedOn(r, g, b);

	public static void on(Color color) => bc.TurnLedOn(color.r, color.g, color.b);

	public staticvoid off() => bc.TurnLedOff();
}
