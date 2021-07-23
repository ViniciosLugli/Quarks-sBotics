public interface IVictim{
	Vector2 position { get; set; }
	sbyte priority { get; set; }
}

public struct AliveVictim: IVictim{
	public AliveVictim(Vector2 position_, sbyte priority_ = 0){
		this.position = position_;
		this.priority = priority_;
	}

	public Vector2 position { get; set; }
	public sbyte priority { get; set; }
}

public struct DeadVictim: IVictim{
	public DeadVictim(Vector2 position_, sbyte priority_ = 0){
		this.position = position_;
		this.priority = priority_;
	}


	public Vector2 position { get; set; }
	public sbyte priority { get; set; }
}
