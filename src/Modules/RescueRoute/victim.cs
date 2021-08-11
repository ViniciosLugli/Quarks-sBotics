public class Victim {
	private bool isRescued { get; set; } = false;
	public Vector2 position { get; set; }
	public sbyte priority { get; set; }

	private byte id = 0;


	public Victim(Vector2 position_, sbyte priority_) {
		this.position = position_;
		this.priority = priority_;
		this.id = UNIQUEID++;
	}

	public void rescue() {
		if (!isRescued) {
			isRescued = true;
			return;
		}
	}

	public string infos() => $"'position':[{this.position.x},{this.position.y}], 'priority':{this.priority}, 'isRescued':{this.isRescued}, 'id':{this.id}";
}

public class AliveVictim : Victim {
	public AliveVictim(Vector2 position, sbyte priority = 0) : base(position, priority) { }
}
public class DeadVictim : Victim {
	public DeadVictim(Vector2 position, sbyte priority = 1) : base(position, priority) { }
}
