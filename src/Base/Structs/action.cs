public struct Action {
	public Action(bool raw_) {
		this.raw = raw_;
	}

	public bool pressed {
		get => this.raw;
	}

	public bool raw;
}
