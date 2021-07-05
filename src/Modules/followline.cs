public static class FollowLine{
	public static void proc(){
		Log.proc();
		Log.info(BOLD(COLOR($"{s1.light.raw} | {s2.light.raw}", "#FFEA79")));
		Log.debug($"{MARKER("--", s1.light.toHex())} | {MARKER("--", s2.light.toHex())}");
	}
}
