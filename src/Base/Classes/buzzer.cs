import("Base/Structs/sound.cs");

public static class Buzzer{
	public static void play(string note, int time=100, int index = 0) => bc.PlayNote(index, note, time);
	public static void play(Sound sound, int index = 0) => bc.PlayNote(index, sound.note, sound.time);

	public static void stop() => bc.StopSound(0);
}
