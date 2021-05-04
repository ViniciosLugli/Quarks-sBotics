public struct Action{
	public Action(bool raw_){
		this.raw = raw_;
	}

	public bool raw;

	//Basic operators
	public static bool operator ==(Action a, Action b) => a.raw == b.raw;
	public static bool operator !=(Action a, Action b) => a.raw != b.raw;
}
