public static class Log{
	public static void proc(object local, object process) => bc.Print(0, $"<align=\"center\">{COLOR(BOLD(local.ToString()), "#FF6188")} {COLOR(process.ToString(), "#947BAF")}");
	public static void proc(){
		var methodInfo = (new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod();
		bc.Print(0,$"<align=\"center\">{COLOR(BOLD(methodInfo.ReflectedType.Name),"#FF6188")} {COLOR(methodInfo.Name, "#947BAF")}");
	}

	public static void info(object data) => bc.Print(1, "<align=\"center\">"+data.ToString());

	public static void debug(object data) => bc.Print(2, "<align=\"center\">"+data.ToString());

	public static void custom(byte line, object data) => bc.Print((int)line, "<align=\"center\">"+data.ToString());

	public static void clear() => bc.ClearConsole();
}
