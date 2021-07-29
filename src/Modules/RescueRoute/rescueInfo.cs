public struct RescueInfo{
	public sbyte triangle;
	public sbyte exit;

	public void setTriangle(sbyte triangle_){
		if(this.triangle != 0){return;}
		Buzzer.play(sRescueFindArea);
		Log.info($"Rescue triangle on: {triangle_}");
		this.triangle = triangle_;
		RescueAnalyzer.exportRescue(this);
	}

	public void setExit(sbyte exit_){
		if(this.exit != 0){return;}
		Buzzer.play(sRescueFindArea);
		Log.info($"Rescue exit on: {exit_}");
		this.exit = exit_;
		RescueAnalyzer.exportRescue(this);
	}

	public bool hasInfos(){
		return this.triangle != 0 && this.exit != 0;
	}
}
