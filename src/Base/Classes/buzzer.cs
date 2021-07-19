import("Base/Structs/sound.cs");

public static class Buzzer{
	public static void play(string note, int time=100) => bc.PlayNote(0, note, time);
	public static void play(Sound sound) => bc.PlayNote(0, sound.note, sound.time);

	public static void stop() => bc.StopSound(0);
}
