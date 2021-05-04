public struct Sound{
	public Sound(string note_, int time_){
		this.note = note_;
		this.time = time_;
	}

	public string note;
	public int time;

	//Basic operators
	public static bool operator ==(Sound a, Sound b) => a.note == b.note;
	public static bool operator !=(Sound a, Sound b) => a.note != b.note;
}
