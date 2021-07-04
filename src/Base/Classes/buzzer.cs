import("Base/Structs/sound.cs");

public static class Buzzer{
	public static Sound play(string note, int time=100){bc.PlayNote(1, note, time);return new Sound(note, time);}
	public static Sound play(Sound sound){bc.PlayNote(1, sound.note, sound.time);return new Sound(sound.note, sound.time);}

	public static void stop() => bc.StopSound(1);
}
