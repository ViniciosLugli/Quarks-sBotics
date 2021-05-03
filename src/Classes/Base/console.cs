class Log{
	static public void proc(object data) => bc.PrintConsole(1, data.ToString());

	static public void info(object data) => bc.PrintConsole(2, data.ToString());

	static public void debug(object data) => bc.PrintConsole(3, data.ToString());

	static public void custom(byte line, object data) => bc.PrintConsole((int)line, data.ToString());

	static public void clear() => bc.ClearConsole();
};
