public static class RescueAnalyzer{

	public static void setup(){
		bc.EraseConsoleFile();
		bc.SetFileConsolePath("/home/vinicioslugli/Documentos/scripts/python/sBotics-viewer/res/out.txt");
	}

	public static void exportVictim(AliveVictim victim) => bc.WriteText($"[ALIVEVICTIM]({victim.infos()})");
	public static void exportVictim(DeadVictim victim) => bc.WriteText($"[DEADVICTIM]({victim.infos()})");
	public static void exportPoint(Vector2 vector, string color, string info = "") => bc.WriteText($"[POINT]('position':[{vector.x},{vector.y}],'color':{color},'info':{info})");
	public static void exportLine(Vector2 vector1, Vector2 vector2, string color, string info = "") => bc.WriteText($"[LINE]('position1':[{vector1.x},{vector1.y}],'position2':[{vector2.x},{vector2.y}],'color':{color},'info':{info})");
	public static void exportRescue(RescueInfo rescue) => bc.WriteText($"[RESCUE]('triangle':{rescue.triangle}, 'exit':{rescue.exit})");
	public static void exportClearLines() => bc.WriteText($"[CLEARLINES]()");
}
