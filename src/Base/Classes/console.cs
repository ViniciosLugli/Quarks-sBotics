class Log{
	public static void proc(object local, object process) => bc.Print(1, $"{COLOR(BOLD(local.ToString()), "#6E6E6E")} | {COLOR(process.ToString(), "#ADADAD")}");
	public static void info(object data) => bc.Print(2, data.ToString());

	public static void debug(object data) => bc.Print(3, data.ToString());

	public static void custom(byte line, object data) => bc.Print((int)line, data.ToString());

	public static void clear() => bc.ClearConsole();
}
