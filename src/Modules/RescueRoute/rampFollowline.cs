public class RampFollowLine{
	public RampFollowLine(ref Reflective refs1_, ref Reflective refs2_, int velocity_){
		this.s1 = refs1_;
		this.s2 = refs2_;
		this.velocity = velocity_;
	}

	public Reflective s1, s2;
	public int velocity = 0;

	private void debugSensors(){
		Log.info(Formatter.parse($"{this.s1.light.raw} | {this.s2.light.raw}", new string[] { "align=center", "color=#FFEA79", "b" }));
		Led.on(cRampFollowLine);
	}

	public void proc(){
		Log.proc();
		this.debugSensors();
	}
}
