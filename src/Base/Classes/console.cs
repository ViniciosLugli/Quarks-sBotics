public static class Log{
	public static void proc(object local, object process) => bc.Print(0, Formatter.parse($"{Formatter.parse(local.ToString(),new string[]{"color=#FF6188", "b"})} {Formatter.parse(process.ToString(),new string[]{"color=#947BAF", "b"})}", new string[]{"align=center"}));
	public static void proc(){
		var methodInfo = (new StackTrace()).GetFrame(1).GetMethod();
		bc.Print(0, Formatter.parse($"{Formatter.parse(methodInfo.ReflectedType.Name,new string[]{"color=#FF6188", "b"})} {Formatter.parse(methodInfo.Name,new string[]{"color=#947BAF", "b"})}", new string[]{"align=center"}));
	}

	public static void info(object data) => bc.Print(1, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void debug(object data) => bc.Print(2, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void custom(byte line, object data) => bc.Print((int)line, Formatter.parse(data.ToString(), new string[]{"align=center"}));

	public static void clear() => bc.ClearConsole();
}
