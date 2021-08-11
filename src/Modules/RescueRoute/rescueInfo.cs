public struct RescueInfo {
	public sbyte triangle;
	public sbyte exit;

	public int triangleBaseDegrees() {
		if (this.triangle == 1) {
			return 135;
		} else if (this.triangle == 2) {
			return 45;
		} else if (this.triangle == 3) {
			return -45;
		}
		return 0;
	}

	public bool setTriangle(sbyte triangle_) {
		if (this.triangle != 0) { return false; }
		Buzzer.play(sRescueFindArea);
		Log.info($"Rescue triangle on: {triangle_}");
		this.triangle = triangle_;
		RescueAnalyzer.exportRescue(this);
		return true;
	}

	public bool setExit(sbyte exit_) {
		if (this.exit != 0) { return false; }
		Buzzer.play(sRescueFindArea);
		Log.info($"Rescue exit on: {exit_}");
		this.exit = exit_;
		RescueAnalyzer.exportRescue(this);
		return true;
	}

	public bool hasInfos() {
		return this.triangle != 0 && this.exit != 0;
	}
}
